
// $('.datepicker')
//        .datepicker({
//            language: "pt-BR",
//            todayHighlight: true
//        });


//function sucesso(data) {
//    $('#form').each(function () {
//        this.reset();
//    });
//    ExibirNotificacaoSucesso('Dados inseridos com sucesso');
//};


//function falha(data) {
//    var msg = data.responseText;
//    var msg2 = $(msg).find('h2').text();
//    ExibirNotificacaoErro('Um erro ocorreu ao executar ação.<br>'+msg2);
//};



function ExibirNotificacaoErro(notificacoes) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "8000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
    toastr["error"](notificacoes, "Erro");
}

function ExibirNotificacaoInfo(notificacoes) {
     
        toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "8000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
        };
        toastr["info"](notificacoes, "Informação");

}


function ExibirNotificacaoSucesso(notificacoes) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "8000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
    toastr["success"](notificacoes, "Informação");

}

function ExibirNotificacaoAtencao(notificacoes) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "8000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
    toastr["warning"](notificacoes, "Atenção");
}

function chkSDSelecionarTodos(chkboxName, statusId) {
    $(".chkSD-" + statusId).prop("checked", $("#" + chkboxName).prop("checked"));
}
