window.fabricInterop = {
    canvas: null,
    isEditable: false,
    dotNetHelper: null,
    debounceTimer: null,
    isPopulating: false,

    initCanvas: function (canvasId) {
        if (this.canvas) {
            this.canvas.dispose();
            this.canvas = null;
        }

        this.canvas = new fabric.Canvas(canvasId, {
            width: document.getElementById('canvas-wrapper').clientWidth,
            height: document.getElementById('canvas-wrapper').clientHeight,
            backgroundColor: '#ffffff'
        });

        window.addEventListener('resize', () => {
            if (this.canvas) {
                this.canvas.setWidth(document.getElementById('canvas-wrapper').clientWidth);
                this.canvas.setHeight(document.getElementById('canvas-wrapper').clientHeight);
                this.canvas.renderAll();
            }
        });
        return true;
    },

    setupEditableCanvas: function (editable, dotNetRef) {
        this.isEditable = editable;
        this.dotNetHelper = dotNetRef;
        this.canvas.selection = this.isEditable;
        this.canvas.isDrawingMode = false;
        if (this.isEditable) {
            this.canvas.on('object:modified', this.contentChanged.bind(this));
            this.canvas.on('object:added', this.contentChanged.bind(this));
            this.canvas.on('object:removed', this.contentChanged.bind(this));
        }
        return true;
    },

    setupViewOnlyCanvas: function () {
        this.isEditable = false;
        this.canvas.selection = false;
        this.canvas.isDrawingMode = false;
        this.canvas.forEachObject((obj) => {
            obj.selectable = false;
            obj.evented = false;
        });
        return true;
    },

    contentChanged: function () {
        if (this.isPopulating) return;
        clearTimeout(this.debounceTimer);
        this.debounceTimer = setTimeout(() => {
            const json = JSON.stringify(this.canvas.toJSON());
            this.dotNetHelper.invokeMethodAsync('CanvasContentChanged', json);
        }, 300);
    },

    loadFromJSON: function (json) {
        if (!this.canvas) {
            console.error('Canvas not initialized');
            return false;
        }
        try {
            this.isPopulating = true;
            this.canvas.clear();
            this.canvas.backgroundColor = '#ffffff';
            const wrapper = document.getElementById('canvas-wrapper');
            if (wrapper) {
                this.canvas.setWidth(wrapper.clientWidth);
                this.canvas.setHeight(wrapper.clientHeight);
            }
            if (!json || json === '{}' || json === '') {
                console.warn('Empty JSON provided to loadFromJSON');
                this.canvas.renderAll();
                this.isPopulating = false;
                return true;
            }
            const jsonObj = typeof json === 'string' ? JSON.parse(json) : json;
            this.canvas.loadFromJSON(jsonObj, () => {
                console.log('JSON loaded successfully');
                this.canvas.calcOffset();
                this.canvas.renderAll();

                if (!this.isEditable) {
                    this.canvas.forEachObject((obj) => {
                        obj.selectable = false;
                        obj.evented = false;
                    });
                }
                this.isPopulating = false;
            });
            return true;
        } catch (error) {
            console.error('Error loading canvas:', error);
            console.error('JSON that caused error:', json);
            this.isPopulating = false;
            this.canvas.clear();
            this.canvas.backgroundColor = '#ffffff';
            this.canvas.renderAll();
            return false;
        }
    },

    clearCanvas: function () {
        if (!this.canvas) return false;
        this.canvas.clear();
        this.canvas.backgroundColor = '#ffffff';
        this.canvas.renderAll();
        return true;
    },

    addTextBlock: function (text) {
        if (!this.canvas || !this.isEditable) return false;

        const textObj = new fabric.IText(text || 'Double-click to edit', {
            left: 50,
            top: 50,
            fontFamily: 'Arial',
            fontSize: 20,
            fill: '#000000'
        });
        this.canvas.add(textObj);
        this.canvas.setActiveObject(textObj);
        this.canvas.renderAll();
        this.contentChanged();
        return true;
    },

    addRectangle: function () {
        if (!this.canvas || !this.isEditable) return false;

        const rect = new fabric.Rect({
            left: 100,
            top: 100,
            width: 100,
            height: 100,
            fill: '#3498db',
            stroke: '#2980b9',
            strokeWidth: 2
        });
        this.canvas.add(rect);
        this.canvas.setActiveObject(rect);
        this.canvas.renderAll();
        this.contentChanged();
        return true;
    },

    addCircle: function () {
        if (!this.canvas || !this.isEditable) return false;

        const circle = new fabric.Circle({
            left: 150,
            top: 150,
            radius: 50,
            fill: '#e74c3c',
            stroke: '#c0392b',
            strokeWidth: 2
        });
        this.canvas.add(circle);
        this.canvas.setActiveObject(circle);
        this.canvas.renderAll();
        this.contentChanged();
        return true;
    },

    deleteSelected: function () {
        if (!this.canvas || !this.isEditable) return false;
        const activeObject = this.canvas.getActiveObject();
        if (activeObject) {
            this.canvas.remove(activeObject);
            this.canvas.renderAll();
            this.contentChanged();
            return true;
        }
        return false;
    },

    changeObjectColor: function (color) {
        if (!this.canvas || !this.isEditable) return false;
        const activeObject = this.canvas.getActiveObject();
        if (!activeObject) return false;
        
        if (activeObject.type === 'path') {
            activeObject.set('stroke', color);
        } else {
            if (activeObject.type === 'i-text' || activeObject.type === 'text') {
                activeObject.set('fill', color);
            } else {
                activeObject.set('fill', color);
                if (activeObject.stroke) {
                    activeObject.set('stroke', this.getDarkerColor(color));
                }
            }
        }
        this.canvas.renderAll();
        this.contentChanged();
        return true;
    },

    getDarkerColor: function(hexColor) {
        const r = parseInt(hexColor.slice(1, 3), 16);
        const g = parseInt(hexColor.slice(3, 5), 16);
        const b = parseInt(hexColor.slice(5, 7), 16);
        const darkerR = Math.max(0, Math.floor(r * 0.8));
        const darkerG = Math.max(0, Math.floor(g * 0.8));
        const darkerB = Math.max(0, Math.floor(b * 0.8));
        return '#' + 
            darkerR.toString(16).padStart(2, '0') +
            darkerG.toString(16).padStart(2, '0') +
            darkerB.toString(16).padStart(2, '0');
    },

    enableDrawingMode: function () {
        if (!this.canvas || !this.isEditable) return false;
        this.canvas.isDrawingMode = true;
        this.canvas.off('path:created');
        this.canvas.on('path:created', this.contentChanged.bind(this));
        this.canvas.off('object:modified');
        return true;
    },

    disableDrawingMode: function () {
        if (!this.canvas || !this.isEditable) return false;
        this.canvas.isDrawingMode = false;
        this.canvas.off('object:modified');
        this.canvas.on('object:modified', this.contentChanged.bind(this));
        return true;
    }
};