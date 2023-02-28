
function loadNoteDetail(id) { // modal yüklenirken 
    $("#modal_notedetail .modal-body").empty(); // içini boşalt sonra
    var loading = $(".template > .loading").html();


    $("#modal_notedetail .modal-body").append(loading); // yükleniyor elementini koy 
    $("#modal_notedetail .modal-body").load("/Home/GetNoteDetail/" + id); // ve istek atınca ve içi dolunca bize göster
}



function loadNoteComments(id) { // modal yüklenirken 
    $("#modal_notecomments .modal-body").empty(); // içini boşalt sonra
    var loading = $(".template > .loading").html();


    $("#modal_notecomments .modal-body").append(loading); // yükleniyor elementini koy 
    $("#modal_notecomments .modal-body").load("/Home/GetNoteComments/" + id); // ve istek atınca ve içi dolunca bize göster
}


function sendComment(id) { // modal yüklenirken 
    var txt = $("#txtCommentText").val();

    $("#modal_notecomments .modal-body").empty(); // içini boşalt sonra
    var loading = $(".template > .loading").html();


    $("#modal_notecomments .modal-body").append(loading); // yükleniyor elementini koy


    $.ajax({
        method: "post",
        url: "/Note/AddCommentToNote/" + id,
        data: { text: txt }

    }).done(function (result) {
        if (result.hasError != undefined && result.hasError) {
            alert("Yorumunuz eklenmedi. Bir hata oluştu");
        }

    }).fail(function () {

        alert("Yorumunuz eklenmedi. Bir hata oluştu");

    }).always(function () {
        $("#modal_notecomments .modal-body").load("/Home/GetNoteComments/" + id); // ve istek atınca ve içi dolunca bize göster

    });

}


function removeComment(commentId, noteId) {
    var conf = confirm("Yorumunuzu silmek istediğinize emin misiniz?");

    if (conf) {

        $("#modal_notecomments .modal-body").empty(); // içini boşalt sonra
        var loading = $(".template > .loading").html();


        $("#modal_notecomments .modal-body").append(loading); // yükleniyor elementini koy


        $.ajax({
            method: "post",
            url: "/Note/RemoveComment/" + commentId

        }).done(function (result) {
            if (result.hasError != undefined && result.hasError) {
                alert("Yorumunuz silinemedi. Bir hata oluştu");
            }

        }).fail(function () {

            alert("Yorumunuz silinemedi. Bir hata oluştu");

        }).always(function () {
            $("#modal_notecomments .modal-body").load("/Home/GetNoteComments/" + noteId); // ve istek atınca ve içi dolunca bize göster

        });

    }
}


function editComment(editlink) {
    var mbody = $(editlink).parent().parent().parent();

    mbody.find(".edit-button").addClass("d-none");
    mbody.find(".edit-buttons").removeClass("d-none");
    mbody.find(".edit-delete-button").addClass("d-none");
    mbody.find(".comment-text").attr("contenteditable", "true").focus();
}

function cancelEditComment(cancelEditLink, noteId) {
    //var mbody = $(cancelEditLink).parent().parent().parent().parent();

    //mbody.find(".edit-button").removeClass("d-none");
    //mbody.find(".edit-buttons").addClass("d-none");
    //mbody.find(".comment-text").removeAttr("contenteditable");

    $("#modal_notecomments .modal-body").load("/Home/GetNoteComments/" + noteId);
}

function updateComment(saveEditLink, commentId, noteId) {
    var mbody = $(saveEditLink).parent().parent().parent().parent();
    var parag = mbody.find(".comment-text");
    parag.removeAttr("contenteditable");

    var newCommentText = parag.text();

    $("#modal_notecomments .modal-body").empty(); // içini boşalt sonra
    var loading = $(".template > .loading").html();


    $("#modal_notecomments .modal-body").append(loading); // yükleniyor elementini koy


    $.ajax({
        method: "post",
        url: "/Note/UpdateComment/" + commentId,
        data: { text: newCommentText }

    }).done(function (result) {
        if (result.hasError != undefined && result.hasError) {
            alert("Yorumunuz güncellenemedi. Bir hata oluştu");
        }

    }).fail(function () {

        alert("Yorumunuz güncellenemedi. Bir hata oluştu");

    }).always(function () {
        $("#modal_notecomments .modal-body").load("/Home/GetNoteComments/" + noteId); // ve istek atınca ve içi dolunca bize göster

    });

}


function likeNote(linkBtn, noteId) {

    $.ajax({
        method: "post",
        url: "/Note/LikeNote/" + noteId,

    }).done(function (result) {
        if (result.hasError != undefined && result.hasError) {
            alert("Beğenme işlemi yapılamadı. Bir hata oluştu");
        }
        else {
            var icon = $(linkBtn).find("i"); // linbtn oradan i elementini bul
            icon.toggleClass("fas"); // icon fas ise çıkar far yap far ise çıkar fas yap
            icon.toggleClass("far");

            $(linkBtn).find("span").text(result.likeCount); // spani full ve likecount değiştir
        }

    }).fail(function () {
        alert("Beğenme işlemi yapılamadı. Bir hata oluştu");
    });
}

var $copyContainer = $(".copy-container"),
    $replayIcon = $('#cb-replay'),
    $copyWidth = $('.copy-container').find('p').width();

var mySplitText = new SplitText($copyContainer, { type: "words" })
    splitTextTimeline = new TimelineMax();
var handleTL = new TimelineMax();

// WIP - need to clean up, work on initial state and handle issue with multiple lines on mobile

var tl = new TimelineMax();

tl.add(function () {
    animateCopy();
    blinkHandle();
}, 0.2)

function animateCopy() {
    mySplitText.split({ type: "chars, words" })
    splitTextTimeline.staggerFrom(mySplitText.chars, 0.001, {
        autoAlpha: 0, ease: Back.easeInOut.config(1.7), onComplete: function () {
            animateHandle()
        }
    }, 0.05);
}

function blinkHandle() {
    handleTL.fromTo('.handle', 0.4, { autoAlpha: 0 }, { autoAlpha: 1, repeat: -1, yoyo: true }, 0);
}

function animateHandle() {
    handleTL.to('.handle', 0.7, { x: $copyWidth, ease: SteppedEase.config(12) }, 0.05);
}

$('#cb-replay').on('click', function () {
    splitTextTimeline.restart()
    handleTL.restart()
})





