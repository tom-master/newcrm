$.fn.extend({
    ProgressBarWars: function (opciones) {
        var progressBarWars = this;
        var theidProgressBarWars = $(progressBarWars).attr("id");
        var styleUnique = Date.now();
        var StringStyle = "";

        defaults = {
            porcentaje: "100",
            tiempo: 500,
            color: "",
            estilo: "yoda",
            tamanio: "30%",
            alto: "6px"
        }

        var opciones = $.extend({}, defaults, opciones);
        if (opciones.color != '') {
            StringStyle = "<style>.color" + styleUnique + "{ border-radius: 2px;display: block; width: 0%; box-shadow:0px 0px 10px 1px " + opciones.color + ", 0 0 1px " + opciones.color + ", 0 0 1px " + opciones.color + ", 0 0 1px " + opciones.color + ", 0 0 1px " + opciones.color + ", 0 0 1px " + opciones.color + ", 0 0 1px " + opciones.color + ";background-color: #fff;}</style>"; opciones.estilo = "color" + styleUnique;
        }
        $(progressBarWars).before(StringStyle);
        $(progressBarWars).append('<span class="barControl" style="width:' + opciones.tamanio + ';"><div class="barContro_space"><span class="' + opciones.estilo + '" style="height: ' + opciones.alto + ';"  id="bar' + theidProgressBarWars + '"></span></div></span>');
        //$("#bar" + theidProgressBarWars).animate({ width: opciones.porcentaje + "%" }, 'slow');
        this.mover = function (ntamanio) {
            opciones.porcentaje = ntamanio;
            $("#bar" + $(this).attr("id")).animate({ width: opciones.porcentaje + "%" }, 'fast');
            return opciones.porcentaje;
        };

        this.getCurrentPorcent = function () {
            return opciones.porcentaje;
        }
        return this;
    }
});