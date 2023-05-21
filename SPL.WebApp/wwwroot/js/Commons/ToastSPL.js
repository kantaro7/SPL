let _options = {
    animation: true,
    autohide: true,
    delay: 3000
};

function ShowToastSuccess() {
    var element = document.getElementById("toastSuccess");
    var toast = new bootstrap.Toast(element, _options);

    toast.show();
}


function ShowFailedMessage(detail_message) {
    Swal.fire({
        icon: 'error',
        title: 'Error',
        text: detail_message,
        allowOutsideClick: false,
        allowEscapeKey: false
    });
}

function ShowFailedMessageWithModal(detail_message, isAproved) {
    Swal.fire({
        title: 'Error',
        text: detail_message,
        icon: 'error',
        showCancelButton: false,
        allowOutsideClick: false,
        allowEscapeKey: false,
        confirmButtonText: 'Ok'
    }).then((result) => {
        if (result.isConfirmed) {
            if (!isAproved) {
                dialogo()
            }
        }
    })
}

function ShowSuccessMessage(detail_message, timerB = null) {

    let timerRes = 0;
    if (timerB != null) {
        timerRes = timerB
    } else {
        timerRes = 2500
    }

    const Toast = Swal.mixin({
        toast: true,
        showConfirmButton: false,
        timer: timerRes,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    })
    Toast.fire({
        icon: 'success',
        title: detail_message
    })
}

function ShowWarningMessage(detail_message) {
    Swal.fire({
        icon: 'warning',
        title: 'Atención',
        text: detail_message,
        //html: detail_message,
        allowOutsideClick: false,
        allowEscapeKey: false
    });
}

function ShowWarningMessageHtmlContent(detail_message) {
    var htmlMessage = '<div style="text-align:left;">' + detail_message + '</div>';
    Swal.fire({
        icon: 'warning',
        title: 'Atención',
        html: htmlMessage,
        allowOutsideClick: false,
        allowEscapeKey: false
    });
}