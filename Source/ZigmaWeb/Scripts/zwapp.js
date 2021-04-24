/// <reference path="zwf.js" />

//
// ---------------------------------- ZwApp
//

function ZwApp() {

    var
        /// <field type='Object'>A dictionary that contains user gadgets (key: GadgetInstance.giid, value: GadgetInstance). </field>
        gadgetInstances = {},
        /// <field type='Zwf.User'>The user of the app</field>
        user = new Zwf.User(),
        /// <field type='Zwf.PanelHost'>The only PanelHost object of the app</field>
        ph = null;

    this.run = function () {
        /// <summary>Main method of the application</summary>

        RegisterDomEventHandlers();
        user.getInitialConfiguration(onInitConfigReceive);
    }

    function RegisterDomEventHandlers() {
        /// <summary>Registers some event handlers for html elements of the document. other objects have their own event handlers.</summary>

    }

    function onInitConfigReceive(response) {
        /// <summary>This method is called when initial configuration of the application received.</summary>

        var
            data = response.Data,

            // Parse PanelHost configuration
            phc = $.parseJSON(data.phc);

        // Create PanelHost
        ph = new Zwf.Ui.PanelHost("ph", phc.cw, phc.ch, phc.cg, phc.rows);

        // Create GadgetInstance and Panel objects and add Panels to PanelHost
        for (var i = 0; i < data.ugc.length; i++) {
            var
                g = data.ugc[i],
                panel = new Zwf.Ui.Panel(g.row, g.col, g.rs, g.cs),
                gi = new Zwf.GadgetInstance(g.giid, g.row, g.col, g.rs, g.cs, panel);

            ph.addPanel(panel);
            gi.sName = g.sName;
            gi.pName = g.pName;
            gi.load();
            gadgetInstances[g.giid] = gi;
        }

        ph.beginLayout();
    }
}


function onEnableClick() {
    ph.beginLayout();

}

function onDisableClick() {
    ph.endLayout();
}


//
// ---------------------------------- Run Application
//
$(function () {
    (new ZwApp()).run();
})