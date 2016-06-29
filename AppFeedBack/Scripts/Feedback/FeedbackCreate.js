var fileIndex = 0;

$(document).ready(function() {

    $("#attachFile").click(function(event) {

        $("#file-error").remove();

        event.preventDefault();
        var input = $("<input>");
        var box = $("#input-container");

        input.attr("name", "Files[" + fileIndex + "]");
        input.attr("type", "file");
        input.css = "display: none";

        input.change(function () {
            box.append(input);
            fileIndex++;
            addFilePreview($(this).prop("files")[0], input);
        });

        input.click();

    });
});

function resetFileInputs() {

    fileIndex = 0;
    var inputs = $("input[type=file]");

    for (var i = 0; i < inputs.length; i++) {
        inputs[i].name = "Files[" + i + "]";
        fileIndex++;
    }

};

function addFilePreview(file, input) {

    var container = $("#preview-container");
    var block = $("<div class='preview-wrapper'>");
    var fileBox = $("<div class = 'file-preview-box'>");
    var closeBox = $("<div class = 'delete-preview-box'>");
    var label = $("<span>");
    var image = $("<img style = 'width: 30px;' src = '/Content/Icons/file-icon.png'/>");
    var close = $("<img style = 'width: 16px;' src = '/Content/Icons/close.png'/>");

    close.click(function () {
        input.remove();
        block.remove();
        resetFileInputs();
    });

    block.mouseenter(function () {
        $(this).children().last().show();
    });

    block.mouseleave(function () {
        $(this).children().last().hide();
    });

    var fileSize = (file.size / 1024).toFixed(2);
    var fileSizeStr = fileSize > 1024 ? (fileSize / 1024).toFixed(2) + "МБ" : fileSize + "КБ";

    label.text(file.name + " [ " + fileSizeStr + " ]");

    fileBox.append(image);
    fileBox.append(label);
    closeBox.append(close);

    block.append(fileBox);
    block.append(closeBox);

    container.append(block);
}