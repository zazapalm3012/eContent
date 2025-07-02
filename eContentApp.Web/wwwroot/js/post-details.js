var quill;

$(document).ready(function () {
    quill = new Quill('#editor', {
        theme: 'snow',
        modules: {
            toolbar: {
                container: [
                    ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
                    ['blockquote', 'code-block'],

                    [{ 'header': 1 }, { 'header': 2 }],               // custom button values
                    [{ 'list': 'ordered'}, { 'list': 'bullet' }],
                    [{ 'script': 'sub'}, { 'script': 'super' }],      // superscript/subscript
                    [{ 'indent': '-1'}, { 'indent': '+1' }],          // outdent/indent
                    [{ 'direction': 'rtl' }],                         // text direction

                    [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
                    [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

                    [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
                    [{ 'font': [] }],
                    [{ 'align': [] }],

                    ['link', 'image'], // Add image button

                    ['clean']                                         // remove formatting button
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

    // Set initial content for the editor
    quill.root.innerHTML = $('#post-edit').data('post-content');

    $('#edit-button').on('click', function () {
        $('#post-display').hide();
        $('#post-edit').show();
        quill.root.innerHTML = $('#post-content-display').html(); // Load current content into editor
    });

    $('#cancel-button').on('click', function () {
        $('#post-edit').hide();
        $('#post-display').show();
        // Revert title if changed
        $('#edit-title').val($('#post-title-display').text());
    });

    $('#save-button').on('click', function () {
        var postId = $('#post-id').val();
        var updatedTitle = $('#edit-title').val();
        var updatedContent = quill.root.innerHTML; // Get HTML content from Quill
        var updatedThumbnailUrl = $('#edit-thumbnail').val(); // Get updated thumbnail URL

        var postData = {
            id: postId,
            title: updatedTitle,
            content: updatedContent,
            thumbnailUrl: updatedThumbnailUrl, // Include thumbnail URL
            // Include other necessary fields from your UpdatePostDto if they are required by the API
            // For example, if CategoryIds is required, you'd need to fetch and send them.
            // For now, assuming only title and content are being updated.
            // categoryIds: JSON.parse($('#post-edit').data('post-categories')) // Pass existing categories
        };

        $.ajax({
            url: `${BASE_API_URL}posts/${postId}`,
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(postData),
            success: function () {
                alert('Post updated successfully!');
                $('#post-title-display').text(updatedTitle);
                $('#post-content-display').html(updatedContent);
                // Update displayed thumbnail if it changed
                // You might need to reload the page or update the image source directly
                location.reload(); // Simple reload for now to reflect thumbnail change
                $('#post-edit').hide();
                $('#post-display').show();
            },
            error: function (xhr, status, error) {
                alert(`Error updating post: ${xhr.responseText}`);
                console.error(xhr.responseText);
            }
        });
    });

    $('#edit-upload-thumbnail-button').on('click', function () {
        $('#edit-thumbnail-upload-input').click();
    });

    $('#edit-thumbnail-upload-input').on('change', async function () {
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
                    $('#edit-thumbnail').val(result.url);
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

    $('#delete-button').on('click', function () {
        var postId = $('#post-id').val();
        if (confirm('Are you sure you want to delete this post?')) {
            $.ajax({
                url: `${BASE_API_URL}posts/${postId}`,
                type: 'DELETE',
                success: function () {
                    alert('Post deleted successfully!');
                    window.location.href = '/Home/Index'; // Redirect to post list
                },
                error: function (xhr, status, error) {
                    alert(`Error deleting post: ${xhr.responseText}`);
                    console.error(xhr.responseText);
                }
            });
        }
    });

    // Fetch and display related posts
    fetch(`${BASE_API_URL}posts`)
        .then(response => response.json())
        .then(posts => {
            console.log(posts); // Debugging: Check the posts data
            const currentPostId = $('#post-id').val(); 
            const relatedPostsContainer = $('#related-posts-container');


            const filteredPosts = posts.filter(post => post.id.toLowerCase() !== currentPostId.toLowerCase());

            const shuffledPosts = filteredPosts.sort(() => 0.5 - Math.random());

            const postsToShow = shuffledPosts.slice(0, 4);

            postsToShow.forEach(post => {
                const postCard = `
                    <div class="col-md-3 mb-4">
                        <div class="post-card">
                            <a href="/Home/Details/${post.id}">
                                <img src="${post.thumbnailUrl || ''}" class="img-fluid mb-3" alt="${post.title || ''}">
                            </a>
                            <h2><a href="/Home/Details/${post.id}">${post.title || ''}</a></h2>
                            <div class="post-meta">
                                Published: ${post.publishedAt ? new Date(post.publishedAt).toLocaleDateString() : 'N/A'}
                            </div>
                        </div>
                    </div>
                `;
                relatedPostsContainer.append(postCard);
            });
        })
        .catch(error => {
            console.error('Error fetching related posts:', error);
            alert(`Error fetching related posts: ${error.message}`);
        });
});