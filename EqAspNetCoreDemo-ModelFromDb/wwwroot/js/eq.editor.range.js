(function ($, undefined) {

    $.widget("eqjs.ValueEditor_InRangeEditor", $.eqjs.ValueEditor, {

        _renderEditor: function () {
            var self = this;

            this._dialogDiv = $("#InRangeEditor");

            this._dialogDiv = $('<div></div>', {
                    'class': 'eqjs-ve-dialog'
                })
                .hide()
                .appendTo(self.element);


            this._sliderDiv = $('<div></div>')
                .appendTo(this._dialogDiv);

            var valueSpanDiv = $('<div></div>')
                .css("padding", "4px 2px")
                .appendTo(this._dialogDiv);

            this._rangeSpan = $("<span></span>")
                .appendTo(valueSpanDiv);

            var value = this._getValue();
            var values = value ? value.split(',') : [];
            var value1 = values.length > 0 ? parseInt(values[0]) : 0;
            var value2 = values.length > 1 ? parseInt(values[1]) : 10000;

            this._sliderDiv.slider({
                values: [value1, value2],
                min: 0,
                max: 10000,
                step: 10,
                create: function (event, ui) {
                    self._renderRangeSpan()
                },
                slide: function (event, ui) {
                    self._renderRangeSpan()
                }
            });

            //render dialog
            this._dialogDiv.dialog({
                autoOpen: false,
                draggable: false,
                resizable: false,
                closeOnEscape: false,
                title: "Range",
                dialogClass: 'eq-js-dialog',
                closeText: "x",
                modal: true,
                //width: parentPanel.options.subQueryDialogWidth,
                //minHeight: parentPanel.options.subQueryDialogHeight,
                //zIndex: parentPanel.options.dialogZIndex,
                buttons: [
                    {
                        name: 'eqjs-dialog-button-ok',
                        text: self.getResText('ButtonOK'),
                        click: function () {
                            var value1 = self._sliderDiv.slider("values", 0);
                            var value2 = self._sliderDiv.slider("values", 1);
                            self._setValue(value1 + "," + value2);
                            $(this).dialog("close");
                        }
                    },
                    {
                        name: 'eqjs-dialog-button-cancel',
                        text: self.getResText('ButtonCancel'),
                        click: function () {
                            $(this).dialog("close");
                        }
                    }
                ],
                open: function () {
                    $('.ui-widget-overlay').addClass('eq-js-dialog-overlay');
                    $('body').css('overflow', 'hidden');
                },
                beforeClose: function () {
                    $('.ui-widget-overlay').removeClass('eq-js-dialog-overlay');
                    $('body').css('overflow', 'auto');
                },
                close: function (event, ui) {
                }
            });
        },

        _renderRangeSpan: function() {
            var value1 = this._sliderDiv.slider("values", 0);
            var value2 = this._sliderDiv.slider("values", 1);
            this._rangeSpan.text(value1 + " - " + value2);
        },

        _showEditor: function() {
            this._dialogDiv.dialog('open');
        }
    })
})(jQuery);