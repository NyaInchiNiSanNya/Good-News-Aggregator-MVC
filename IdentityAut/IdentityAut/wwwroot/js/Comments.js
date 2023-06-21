
    document.getElementById('GetComments')
    .addEventListener('click', collapse);

document.getElementById('PostComment')
    .addEventListener('click', sendComment);

    async function collapse() {

        await getComments();

        new bootstrap.Collapse('#Comments', {
            toggle: true
        })
}

async function getComments() {
    articleId = document.getElementById("GetComments").getAttribute("articleId");

    comments = await fetch('https://localhost:7110/Comment/GetComments/?id=' + articleId, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    let resp = await comments.json();

    commentBlock = document.getElementById('CommentsInner');
    commentBlock.innerHTML = "";

    for (let comment of resp) {

        let div = document.createElement("div");

        const dateString = comment.dateTime;
        const dateObject = new Date(dateString);

        const formattedDate = dateObject.toLocaleDateString('en-US');

        div.innerHTML = `<div style="display:flex">
                              <div class="Icontainer block rounded-circle border border-4 border-dark" >
                                       <img class="user-picture" src="data:image/jpeg;base64,${comment.userPicture}" />
                              </div>
                              <div style="width:80%">
                              <div style="padding:0;display:flex;">
                                      <h5>${comment.userName}</h5> 
                                       <p style="padding-left:1rem; font-size: 16px;">${formattedDate}</p>
                              </div>
                                      <p>${comment.text}</p>
                             </div>
                               
                             </div>`
        commentBlock.appendChild(div);

    }
}

async function sendComment() {

    var Text = document.getElementById("commentArea").value;
    var articleId = document.getElementById("GetComments").getAttribute("articleId");

    var comment = {
        id: articleId, 
        text: Text 
        }

    comments = await fetch('https://localhost:7110/Comment/PostComment', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(comment)
    });
    await getComments();
    document.getElementById("commentArea").value = "";
}
    
