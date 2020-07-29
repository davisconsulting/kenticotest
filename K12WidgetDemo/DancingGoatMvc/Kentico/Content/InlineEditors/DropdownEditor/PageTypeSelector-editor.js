(function () {
    window.kentico.pageBuilder.registerInlineEditor("Kentico.PageTypeSelector-editor", {
        init: function (options) {
            var editor = options.editor;
            var dropdown = editor.querySelector(".Kentico.PageTypeSelector");
            dropdown.addEventListener("change", function () {
                var event = new CustomEvent("updateProperty", {
                    detail: {
                        value: dropdown.value,
                        name: options.propertyName
                    }
                });
                editor.dispatchEvent(event);
            });
        }
    });
})();