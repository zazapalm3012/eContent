var quill;

$(document).ready(function () {
    quill = new Quill('#editor', {
        theme: 'snow',
        modules: {
            toolbar: {
                container: [
                    ['bold', 'italic', 'underline', 'strike'],
                    ['blockquote', 'code-block'],
                    [{ 'header': 1 }, { 'header': 2 }],
                    [{ 'list': 'ordered'}, { 'list': 'bullet' }],
                    [{ 'script': 'sub'}, { 'script': 'super' }],
                    [{ 'indent': '-1'}, { 'indent': '+1' }],
                    [{ 'direction': 'rtl' }],
                    [{ 'size': ['small', false, 'large', 'huge'] }],
                    [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
                    [{ 'color': [] }, { 'background': [] }],
                    [{ 'font': [] }],
                    [{ 'align': [] }],
                    ['link', 'image'],
                    ['clean']
                ],
                handlers: {
                    'image': imageHandler
                }
            }
        }
    });

    function imageHandler() {
        var input = document.createElement('input');
        input.setAttribute('type', 'file');
        input.setAttribute('accept', 'image/*');
        input.click();

        input.onchange = async () => {
            var file = input.files[0];
            var formData = new FormData();
            formData.append('file', file);

            try {
                const response = await fetch(`${BASE_API_URL}media/upload`, {
                    method: 'POST',
                    body: formData
                });

                const result = await response.json();
                if (response.ok) {
                    const range = quill.getSelection();
                    quill.insertEmbed(range.index, 'image', result.url);
                    alert('Image uploaded successfully!');
                } else {
                    console.error('Image upload failed:', result.message);
                    alert(`Image upload failed: ${result.message}`);
                }
            } catch (error) {
                console.error('Error uploading image:', error);
                alert(`Error uploading image: ${error.message}`);
            }
        };
    }

    $('#create-post-button').on('click', function () {
        var title = $('#post-title').val();
        var content = quill.root.innerHTML;
        var thumbnailUrl = $('#post-thumbnail').val();
        

        var postData = {
            title: title,
            content: content,
            thumbnailUrl: thumbnailUrl,
            // categoryIds: selectedCategoryIds
        };

        $.ajax({
            url: `${BASE_API_URL}posts`,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(postData),
            success: function (response) {
                alert('Post created successfully!');
                window.location.href = '/Home/Index'; // Redirect to post list
            },
            error: function (xhr, status, error) {
                alert(`Error creating post: ${xhr.responseText}`);
                console.error(xhr.responseText);
            }
        });
    });

    $('#upload-thumbnail-button').on('click', function () {
        $('#thumbnail-upload-input').click();
    });

    $('#thumbnail-upload-input').on('change', async function () {
        var file = this.files[0];
        if (file) {
            var formData = new FormData();
            formData.append('file', file);

            try {
                const response = await fetch(`${BASE_API_URL}media/upload`, {
                    method: 'POST',
                    body: formData
                });

                const result = await response.json();
                if (response.ok) {
                    $('#post-thumbnail').val(result.url);
                    alert('Thumbnail uploaded successfully!');
                } else {
                    console.error('Thumbnail upload failed:', result.message);
                    alert(`Thumbnail upload failed: ${result.message}`);
                }
            } catch (error) {
                console.error('Error uploading thumbnail:', error);
                alert(`Error uploading thumbnail: ${error.message}`);
            }
        }
    });
});