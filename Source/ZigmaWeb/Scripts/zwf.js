/// <reference path="jquery-2.0.3.js" />
/// <reference path="jquery-ui-1.10.3.custom.min.js" />

var Zwf = {
    // Ui Namespace. contains user interface function constructors
    Ui: {},

    // Gadgets namespace. contains gadgets function constructors
    Gadgets: {}
};

//
// ---------------------------------- Zwf.Ui.PanelHost
//
Zwf.Ui.PanelHost = function (hostID, cellWidth, cellHeight, cellGap, panelHostRows) {
    /// <summary>A host object for storing, managing and showing Zwf.Ui.Panel objects</summary>
    /// <param name="hostID" type="String">ID of host element</param>
    /// <param name="cellWidth" type="Number">Width of each cell of the grid</param>
    /// <param name="cellHeight" type="Number">Height of each cell of the grid</param>
    /// <param name="cellGap" type="Number">Gap space bettwen each cell</param>
    /// <param name="rows" type="Number">Number of PanelHost rows</param>

    var

    // ID of the host element (host element is a DIV html element)
    hostID = hostID,

    // Width of cells (in pixel)
    cw = cellWidth,

    // Height of cells (in pixel)
    ch = cellHeight,

    // Gap space bettwen cells (in pixel)
    cg = cellGap,

    // Number of PanelHost rows
    phRows = panelHostRows,

    // JQuery host object
    $host = $("#" + hostID),

    // Is panel in layout edit mode?
    editMode = false,

    // An object that stores information about dragging panel
    drag = {
        // The panel that is dragging
        panel: null,
        // {row, col} object. New location of the panel that is dragging
        newLocation: null
    },

    // A (key:value) collection of panels (key:Panel.id, value:Panel)
    panels = {},

    // An 2D array that contains refrences to the Panel objects
    map = [],

    // Resize box
    $resizeBox = createResizeBox(),

    // Selected panel
    selectedPanel = null;

    this.addPanel = function (p) {
        /// <summary>Adds a panel to this PanelHost</summary>
        /// <param name="p" type="Zwf.Ui.Panel">panel object that should be added</param>
        
        var
            // get rectanglular region of the panel
            panelRect = getRect(p.row, p.col, p.rowSpan, p.colSpan),

            // JQuery "Cover" element - a <div> element inside $panel that plays "cover" role 
            $cover = $("<div>", {
                "class": "cover"
            }).click(p, onPanelCoverClick);


        // Create <div> element that hosts the contents of the panel
        p.$host = $("<div>", {
            id: p.hostID(),
            "class": "panel"
        }).css({
            right: panelRect.x,
            top: panelRect.y,
            width: panelRect.w,
            height: panelRect.h
        }).data({
            panel: p
        }).hide();

        // Add cover to the panel
        p.$host.append($cover);

        // Add Panel to the PanelHost
        $host.append(p.$host);
        p.$host.fadeIn(500);

        // Add panel to panels collection
        panels[p.ID] = p;

        // Add panel to map array
        setMap(p.row, p.col, p.rowSpan, p.colSpan, p);
    }

    this.removePanel = function (p) {
        /// <summary>Removes a panel from this PanelHost</summary>
        /// <param name="p" type="Zwf.Ui.Panel">panel object that should be removed</param>
    }

    this.removeAllPanels = function () {
        /// <summary>Removes all panels from this PanelHost</summary>

    }

    this.beginLayout = function () {
        /// <summary>Begins layout edit</summary>

        // Safe guard
        if (editMode)
            return;

        // Initialize draggable object and attach event handlers
        $(".panel").draggable({
            distance: 5,
            revert: onPanelRevert,
            containment: "parent",
            drag: onDrag,
            start: onDragStart,
            stop: onDragStop
        });

        // Display cover of panels
        $(".cover").css("display", "block");

        // Begin Layout Edit Mode: true
        editMode = true;
    }

    this.rows = function (r) {
        /// <signature>
        /// <summary>Returns number of PanelHost rows</summary>
        /// <returns type="Number" />
        /// </signature>
        /// <signature>
        /// <summary>Set number of PanelHost rows</summary>
        /// <param name="r" type="Number">Number of rows</param>
        /// </signature>

        // Getter method
        if (r === undefined) {
            return phRows;
        }

        // Setter Method
        var rect = getRect(0, 0, r, 1);
        $host.height(rect.h);
    }

    this.endLayout = function () {
        // Safe guard
        if (!editMode)
            return;

        // Hide cover of panels
        $(".cover").css("display", "none");

        // Deselect selected panel (if any panel is selected currently)
        deselectAll();

        $(".panel").draggable("destroy");
        editMode = false;

    }

    function onHostClick() {
        deselectAll();
    }

    function onPanelRevert() {
        /// <summary>Determine 'revert' status of the panel that is being dragged.</summary>
        /// <returns type="Bool" >true, if panel should revert, else false</returns>

        var

        // The Panel object that is dragging
        pnl = drag.panel,

        // JQuery wrapper of Host object of the panel. it is a <div> element
        $pnlHost = drag.panel.$host,


        // Calculate "right" css property of the dragging panel 
        panelRight = Math.abs($host.css("width").replace("px", "") - $pnlHost.css("left").replace("px", "") - $pnlHost.outerWidth()),

        // Calculate location of "HotPoint" of the panel
        hp = {
            x: panelRight + (cw / 2),
            y: parseInt($pnlHost.css("top")) + (ch / 2)
        },

        // Get the cell under "HotPoint"
        hotCell = getCell(hp.x, hp.y);

        // Can The panel (dragging panel) move to the cell under the HotPoint?
        var canMove = true;
        for (var r = 0; (r < pnl.rowSpan) && canMove; r++) {
            for (var c = 0; (c < pnl.colSpan) && canMove; c++) {
                // Get the Panel object that hotCell points to it.
                var p = getMap(hotCell.row + r, hotCell.col + c);
                if ((p !== undefined) && (p.ID != pnl.ID))
                    canMove = false;
            }
        }

        // Panel should revert ?
        var revert;
        if (canMove) {
            drag.newLocation = hotCell;
        } else {
            revert = true;
        }

        return revert;
    }

    function onDrag(event, ui) {
        // In this event handler, 'this' points to the <DIV> (panel) object that is dragging

    }

    function onDragStart(event, ui) {
        // Remove "right' CSS property, so JQuery can drag the element (JQuery use "left" CSS Propert)
        ui.helper.css("right", "");

        // Initialize drag object
        drag.panel = ui.helper.data().panel;
        drag.newLocation = null;

        // Bring drag.panel element up in the $host children
        $host.append(drag.panel.$host);
    }

    function onDragStop(event, ui) {
        var $pnlHost = drag.panel.$host;

        // Move to new location ?
        if (drag.newLocation) {
            // Compute and set "right" css property of the panel
            var panelRight = $host.width() - $pnlHost.position().left - $pnlHost.outerWidth();
            $pnlHost.css("right", panelRight);

            // Animate panel to the new location
            var newRect = getRect(drag.newLocation.row, drag.newLocation.col, 1, 1);
            $pnlHost.animate({
                right: newRect.x,
                top: newRect.y
            });

            // Update map
            setMap(drag.panel.row, drag.panel.col, drag.panel.rowSpan, drag.panel.colSpan, undefined);
            setMap(drag.newLocation.row, drag.newLocation.col, drag.panel.rowSpan, drag.panel.colSpan, drag.panel);

            // Update Panel object
            drag.panel.row = drag.newLocation.row;
            drag.panel.col = drag.newLocation.col;

        } else {
            // The Panel is reverted
            $pnlHost.css("right", getRect(drag.panel.row, drag.panel.col, 1, 1).x);
        }

        // End of drag operation.
        $pnlHost.css("left", "");
        deselectAll();
        selectPanel(drag.panel);
    }

    function setMap(row, col, rowSpan, colSpan, value) {
        /// <summary>Sets the map to the specified value</summary>

        for (var r = 0; r < rowSpan; r++) {
            if (map[r + row] === undefined)
                map[r + row] = [];
            for (var c = 0; c < colSpan; c++) {
                map[r + row][c + col] = value;
            }
        }
    }

    function getMap(row, col) {
        return (map[row] !== undefined) ? map[row][col] : undefined;
    }

    function getRect(row, col, rowSpan, colSpan) {

        // x:X, y:Y, w:Width, h:Height
        return {
            /// <field name='x' type='Number'>X Cordinate of the rectangle</field>
            x: col * (cw + cg),
            /// <field name='y' type='Number'>Y Cordinate of the rectangle</field>
            y: row * (ch + cg),
            /// <field name='w' type='Number'>Width of the rectangle</field>
            w: colSpan * ( cw + cg ) - cg,
            /// <field name='h' type='Number'>Height of the rectangle</field>
            h: rowSpan * ( ch + cg ) - cg
        }
    }
     
    function getCell(x, y) {
        /// <summary>Returns the row and col of a cell that point (x,y) is located in it.</summary>
        /// <param name="x" type="Number">X cordinate of the point</param>
        /// <param name="y" type="Number">Y cordinate of the point</param>

        return {
            row: Math.floor(y / (ch + cg)),
            col: Math.floor(x / (cw + cg))
        }
    }

    function createResizeBox() {
        /// <summary>Creates resize box</summary>
        /// <returns type="JQuery" >JQuery object of the <div> element of the resize box</returns>

        var
            // Create resize box
            $rb = $("<div>", {
                id: "resize-box"
            }),

            // Create left button ("increase width" button)
            $left_button = $("<img>", {
                "class": "btn-resize btn-resize-left",
                title: "افزایش طول",
                src: "Content/Images/triangle-left.png"
            }).click("left", onResizeButtonClick),

            // Create right button ("decrease width" button)
            $right_button = $("<img>", {
                "class": "btn-resize btn-resize-right",
                title: "کاهش طول",
                src: "Content/Images/triangle-right.png"
            }).click("right", onResizeButtonClick),

            // Create up button ("decrease height" button)
            $up_button = $("<img>", {
                "class": "btn-resize btn-resize-up",
                title: "کاهش ارتفاع",
                src: "Content/Images/triangle-up.png"
            }).click("up", onResizeButtonClick),

            // Create down button ("increase width" button)
            $down_button = $("<img>", {
                "class": "btn-resize btn-resize-down",
                title: "افزایش ارتفاع",
                src: "Content/Images/triangle-down.png"
            }).click("down", onResizeButtonClick);

        // Insert buttons to the resize box
        $rb.append($left_button, $right_button, $up_button, $down_button);

        // Handle resize box click event
        $rb.click(function (e) {
            e.stopPropagation();
        })

        return $rb;
    }

    function onPanelCoverClick(e) {
        /// <summary>This event fired when user clicks on a cover of a panel</summary>
        /// <param name="e">event data</param>
        e.stopPropagation();

        // Get the clicked panel
        var pnl = e.data;

        if (pnl === selectedPanel)
            return;

        // Select clicked panel
        deselectAll();
        selectPanel(pnl);
    }

    function onResizeButtonClick(e) {
        var p = selectedPanel;

        // Prevent event propagation 
        e.stopPropagation();

        switch (e.data) {
            case "left":
                // Increase width
                increaseWidth(p);
                break;
            case "right":
                // Decrease width
                decreaseWidth(p);
                break;
            case "down":
                // Increase height
                increaseHeight(p);
                break;
            case "up":
                // Decrease height
                decreaseHeight(p);
                break;
        }
    }

    function selectPanel(p) {
        /// <summary>Selects a panel</summary>
        /// <param name="p" type="Zwf.Ui.Panel">Panel object that should be select</param>

        selectedPanel = p;
        p.$host.append($resizeBox);
        $("#resize-box").fadeIn(500);
        p.$host.addClass("panel-selected");
    }

    function deselectAll() {
        /// <summary>Deselects all panels</summary>
        /// <param name="p" type="Zwf.Ui.Panel">Panel object that should be deselect</param>

        if (selectedPanel) {
            selectedPanel.$host.removeClass("panel-selected");
            $("#resize-box").fadeOut(0);
            selectedPanel = null;
        }
    }

    function increaseWidth(p) {
        /// <summary>Increase the width of a panel by one row</summary>
        /// <param name="p" type="Zwf.Ui.Panel">The Panel object</param>

        var col = p.col + p.colSpan,
            lastRow = p.row + p.rowSpan - 1,
            doResize = true;

        // Increasing width is possible?
        for (row = p.row; row <= lastRow; row++) {
            if (getMap(row, col)) {
                doResize = false;
                break;
            }
        }

        if (doResize) {
            // Update panel and map
            p.colSpan++;
            setMap(p.row, p.col, p.rowSpan, p.colSpan, p);

            // Update GUI (Panel DOM)
            updatePanelDom(p);
        }
    }

    function decreaseWidth(p) {
        /// <summary>Decrease the width of a panel by one row</summary>
        /// <param name="p" type="Zwf.Ui.Panel">The Panel object</param>

        if (p.colSpan > 1) {
            // Update panel and map
            lastCol = p.col + p.colSpan - 1;
            setMap(p.row, lastCol, p.rowSpan, 1, undefined);
            p.colSpan--;

            // Update GUI (Panel DOM)
            updatePanelDom(p);
        }
    }

    function increaseHeight(p) {
        /// <summary>Increase the height of a panel by one col</summary>
        /// <param name="p" type="Zwf.Ui.Panel">The Panel object</param>

        var row = p.row + p.rowSpan,
            lastCol = p.col + p.colSpan - 1,
            doResize = true;

        // Increasing height is possible?
        if (row >= phRows)
            doResize = false;

        for (col = p.col; (doResize) && (col <= lastCol) ; col++) {
            if (getMap(row, col)) {
                doResize = false;
            }
        }

        if (doResize) {
            // Update panel and map
            p.rowSpan++;
            setMap(p.row, p.col, p.rowSpan, p.colSpan, p);

            // Update GUI (Panel DOM)
            updatePanelDom(p);
        }
    }

    function decreaseHeight(p) {
        /// <summary>Decrease the width of a panel by one col</summary>
        /// <param name="p" type="Zwf.Ui.Panel">The Panel object</param>

        if (p.rowSpan > 1) {
            // Update panel and map
            var lastRow = p.row + p.rowSpan - 1;
            setMap(lastRow, p.col, 1, p.colSpan, undefined);
            p.rowSpan--;

            // Update GUI (Panel DOM)
            updatePanelDom(p);
        }

    }

    function updatePanelDom(p) {
        /// <summary>Updates size and location of the panel host ($host)</summary>
        /// <param name="p" type="Zwf.Ui.Panel">Panel that the size and location of it's $host should be updated</param>

        var r = getRect(p.row, p.col, p.rowSpan, p.colSpan);
        p.$host.animate({
            width: r.w,
            height: r.h
        }, 200);

    }

    // Initialize the object
    $host.click(onHostClick);
    this.rows(phRows);
}

//
// ---------------------------------- Zwf.Ui.Panel
//
Zwf.Ui.Panel = function (row, col, rowSpan, colSpan) {
    /// <field name="$host" type="jQuery">Jquery object of the <div> element of the panel</field>

    Zwf.Ui.Panel.ID = (Zwf.Ui.Panel.ID == undefined ? 0 : Zwf.Ui.Panel.ID + 1);
    this.ID = Zwf.Ui.Panel.ID;
    this.row = row;
    this.col = col;
    this.rowSpan = rowSpan;
    this.colSpan = colSpan;
}

Zwf.Ui.Panel.prototype = {
    hostID: function () {
        return "pnl-" + this.ID;
    }
}

//
// ---------------------------------- Zwf.Gadget
//

Zwf.GadgetInstance = function (giid, row, col, rowSpan, colSpan, panel) {
    /// <summary>GadgetInstance function constructor</summary>
    /// <param name="giid" type="Number">Gadget Instance Id</param>
    /// <param name="panel" type="Zwf.Ui.Panel">Panel object that contains this GadgetInstance</param>
    /// <field name="panel" type="Zwf.Ui.Panel">Panel object that contains this GadgetInstance</field>
    /// <field name="gadget" type='Zwf.Gadgets.[GadgetSystemName]'>Zwf.Gadgets.[GadgetSystemName] object</field>
    /// <field name="sName" type='String'>Gadget system name</field>
    /// <field name="pName" type='String'>Gadget public name</field>

    this.giid = giid;
    this.row = row;
    this.col = col;
    this.rowSpan = rowSpan;
    this.colSpan = colSpan;
    this.panel = panel;
    this.sName = null;
    this.pName = null;
    this.gadget = null;
}

Zwf.GadgetInstance.prototype = {
    load: function (onSuccessCallback) {
        /// <summary>Loads "gadget.html" asynchronously into the panel and runs gadget initialization code (init() method)</summary>
        /// <param name="onSuccessCallback" type="Function(Zwf.GadgetInstance, data)">A callback function that is called when data received</param>
        var gi = this;

        // Send HTTP POST request
        $.get("Gadgets/" + this.sName + "/gadget.html")
            .success(function (data) {
                // Inject gadget.html into <div> element 
                gi.panel.$host.append(data);

                // Create an Zwf.Gadgets.[GadgetSystemName] object and run it
                gi.gadget = new Zwf.Gadgets[gi.sName];
                gi.gadget.init();

                if (onSuccessCallback !== undefined)
                    onSuccessCallback(gi, data);
            });

    },

    getSetting: function (key, onDataRecive) {

    },

    getGlobalSetting: function (key, onDataReciveCallback) {
    },

    addOrUpdateSetting: function (key, value, onDataReciveCallback) {
    },

    UpdateSetting: function (key, value, onDataReciveCallback) {
    },

    RemoveSetting: function (key, onDataReciveCallback) {

    }
}

//
// ---------------------------------- Zwf.User
//
Zwf.User = function () {
    // Gadget Instance ID
    this.firstname;
}

Zwf.User.prototype = {
    getSetting: function (key, onDataReciveCallback) {

    },

    getInitialConfiguration: function (onDataReceiveCallback) {
        /// <summary>Sends a request to recive initial configuration of the user</summary>
        /// <param name='onDataReceiveCallback' type='Function(configData)'>A callback function that is called when data received.</param>
        $.post("api/uc/gic", "", onDataReceiveCallback);
    },

    UpdateOrAddSetting: function (key, value, onDataReciveCallback) {
    },

    UpdateSetting: function (key, value, onDataReciveCallback) {
    },

    RemoveSetting: function (key, onDataReciveCallback) {

    }
}

