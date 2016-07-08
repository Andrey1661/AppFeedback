var fileIndex = 0;

$(document).ready(function() {

    //Клик по кнопке "Прикрепить файл"
    $("#attachFile").click(function(event) {

        $("#file-error").remove();

        event.preventDefault();
        var input = $("<input>");
        var box = $("#input-container");

        input.attr("name", "Files[" + fileIndex + "]");
        input.attr("type", "file");
        input.css = "display: none";

        input.change(function () {
            var file = $(this).prop("files")[0];

            if (validateFile(file)) {
                box.append(input);
                fileIndex++;
                addFilePreview(file, input);
                validateFiles();
            }           
        });

        input.click();
    });

    $("form").submit(function(event) {
        validateFiles(event);
    });
});

//Проверка загружаемого файла
function validateFile(file) {
    var box = $("#file-validation-box");
    var inputs = $("input[type=file]");

    $("#file-validation-message").remove();
    var msg = $("<span id ='file-validation-message' class = 'text-danger'>");

    //true, если файл с таким именем уже добавлен
    var exists = false;
    //true, если файл превышает максимально допустимый размер (5МБ)
    var bigsize = false;

    for (var i = 0; i < inputs.length; i++) {
        if (file.name == inputs[i].files[0].name) {
            msg.html("Файл уже добавлен");
            exists = true;
            break;
        }
    }

    if (file.size > 5242880) {
        msg.html("Размер файла превышает максимальное значение 5МБ");
        bigsize = true;
    }

    if (!(bigsize || exists)) {
         return true;
    }

    box.append(msg);

    setTimeout(function() {
        msg.animate({
            opacity: 0
        }, 1000, function() {
            this.remove();
        });
    }, 3000);

    return false;
}

//Проверяет все прикрепленные файлы на превышения максимально допустимого суммарного размера (15МБ)
function validateFiles(event) {
    var inputs = $("input[type=file]");

    var totalSize = 0;
    var maxSize = 15728640;

    for (var i = 0; i < inputs.length; i++) {
        totalSize += inputs[i].files[0].size;
    }

    $("#file-validation-message-summary").remove();

    if (totalSize > maxSize) {
        if (event !== undefined) {
            event.preventDefault();
        }
        var msg = $("<span id ='file-validation-message-summary' class = 'text-danger'>");
        var box = $("#file-validation-box");

        var sizeStr = (maxSize / 1000000).toFixed(0) + "МБ";
        msg.html("Общий объем передаваемых файлов не должен превышать " + sizeStr);
        box.append(msg);
    }
}

//Переназначает имена элементам input[type=file] при удалении прикрепленного файла
function resetFileInputs() {

    fileIndex = 0;
    var inputs = $("input[type=file]");

    for (var i = 0; i < inputs.length; i++) {
        inputs[i].name = "Files[" + i + "]";
        fileIndex++;
    }

    validateFiles();
};

//При добавлении файла создает preview с именем и рамером этого файла
function addFilePreview(file, input) {

    var container = $("#preview-container");
    var block = $("<div class='preview-wrapper'>");
    var fileBox = $("<div class = 'file-preview-box'>");
    var closeBox = $("<div class = 'delete-preview-box'>");
    var label = $("<span>");
    var image = $("<img style = 'width: 30px;' src = '/Content/Icons/file-icon.png'/>");
    var close = $("<img style = 'width: 16px;' src = '/Content/Icons/cross.png'/>");

    close.mouseenter(function() {
        $(this).attr("src", "/Content/Icons/cross-active.png");
    });
    close.mouseleave(function() {
        $(this).attr("src", "/Content/Icons/cross.png");
    });

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