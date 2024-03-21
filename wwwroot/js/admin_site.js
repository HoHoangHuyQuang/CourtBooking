//front image url
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        
        reader.onload = function (e) {
            $('#image_upload_preview').attr('src', e.target.result);         
 
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$("#customFile").on("change", function () {
    
    readURL(this);
    var fileName = $(this).val().split("\\").pop();
    console.log(fileName);
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});