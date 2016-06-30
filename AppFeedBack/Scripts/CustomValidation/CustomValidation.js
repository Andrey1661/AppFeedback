
//NotNullAttribute---------------------------------------------------
$.validator.addMethod("notempty", function (value, element, params) {
    return value !== params;
});


$.validator.unobtrusive.adapters.add("notempty", ["empty"], function (options) {
    options.rules["notempty"] = options.params.empty;
    options.messages.notempty = options.message;
});
//-------------------------------------------------------------------