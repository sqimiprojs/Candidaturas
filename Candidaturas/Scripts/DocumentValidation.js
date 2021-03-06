﻿$(document).ready(function () {
    $("#newuserform").submit(function () {
        var valid = true;

        if (!validarDoc($("#TipoDoc").val())) {
            valid = false;
        }

        if (!validarDocValidade()) {
            valid = false;
        }

        if (!validarEmail()) {
            valid = false;
        }

        if (!verificarEmail()) {
            valid = false;
        }
        if (!validarIdd()) {
            valid = false;
        }

        if (!valid) {
            return false;
        } else {
            return true;
        }
    })    

    $("#formDadosPessoais").submit(function () {
        var valid = true;

        if (!validarDoc($("#TipoDoc").val())) {
            valid = false;
        }

        if (!validarDocValidade()) {
            valid = false;
        }

        if (!validarIdd()) {
            valid = false;
        }

        if (!validarNIF($("#NIF").val())) {
            valid = false;
        }

        if (!limitarCodPostal3Dig()) {
            valid = false;
        }

        if (!limitarCodPostal4Dig()) {
            valid = false;
        }

        if (!valid) {
            return false;
        } else {
            return true;
        }
    }) 

    $("#NIF").change(function () {
        if ($("#Nacionalidade").val() === "PT") {
            var nif = $("#NIF").val();
            validarNIF(nif);
        }
    });

    $("#Email").blur(function () {
        validarEmail();
        verificarEmail();
    });

    $("#TipoDoc").change(function () {
        var docType = $("#TipoDoc").val();
        if (docType !== "") {
            validarDoc(docType);
        }
    });

    $("#NDI").change(function () {
        var docType = $("#TipoDoc").val();
        if (docType !== "") {
            validarDoc(docType);

        }
    });

    $("#DocVali").focusout(function () {
        validarDocValidade();
    });

    $("#DtNasc").focusout(function () {
        validarIdd();
    });

    $("#user_Militar").change(function () {
        validarIdd();
    });

    $("#CodigoPostal4Dig").change(function () {

        if (limitarCodPostal4Dig() && limitarCodPostal3Dig()) {
            return true;
        } else {
            return false;
        }
    });

    $("#CodigoPostal3Dig").change(function () {
        if (limitarCodPostal4Dig() && limitarCodPostal3Dig()) {
            return true;
        } else {
            return false;
        }    });

    $("#Nacionalidade").change(function () {
        checkNational();
    });

    function HideShowCCWarning(number) {
        if (validarCC(number) && validarDigitosCC(number)) {
            $("#NDIWarning").text("");
            $("#NDIWarning").hide();
            return true;
        }
        else {
            $("#NDIWarning").text("Número de Cartão de Cidadão inválido");
            $("#NDIWarning").show();
            return false;
        }
    }

    function HideShowBIWarning(number) {
        if (validarBI(number) && validarDigitosBI(number)) {
            $("#NDIWarning").text("");
            $("#NDIWarning").hide();
            return true;
        }
        else {
            $("#NDIWarning").text("Número de Bilhete de Identidade inválido");
            $("#NDIWarning").show();
            return false;
        }
    }

    function validarDoc(docType) {
        var number = $("#NDI").val();
        if (number === "") {
            mostrarPlaceholderFormatoCCouBI(docType);
            return false;
        }
        if (docType == 2) {
            return HideShowBIWarning(number);
        }
        else if (docType == 4) {
            return HideShowCCWarning(number);
        } else {
            $("#NDIWarning").text("");
            $("#NDIWarning").hide();
            return true;
        }
    }

    function validarDigitosBI(value) {
        var firstLength = value.length - 1;
        var firstEight = value.substring(0, firstLength);
        var lastDigit = value[firstLength];
        var sum = 0;
        var i;

        for (i = 0; i < firstEight.length; i++) {
            sum += firstEight[firstEight.length - (i + 1)] * (i + 2);
        }

        var res = sum % 11;

        if (res == 0 || res == 1) {
            res = 0;
        }
        else {
            res = 11 - res;
        }

        return res == lastDigit;
    }

    function validarBI(value) {
        var re = new RegExp("^[0-9]{8}[ -]*[0-9]$");
        return re.test(value);
    }

    function validarCC(value) {
        if (value.length == 13) {
            var re = new RegExp("^[0-9]{8}[ -]*[0-9][A-Za-z]{2}[0-9]$");
            return re.test(value);
        } else if (value.length == 12) {
            var re = new RegExp("^[0-9]{7}[ -]*[0-9][A-Za-z]{2}[0-9]$");
            return re.test(value);
        } else {
            var re = new RegExp("^[0-9]{6}[ -]*[0-9][A-Za-z]{2}[0-9]$");
            return re.test(value);
        }
    }

    function validarDigitosCC(value) {
        var aux = value.replace('-', '');
        var sum = 0;
        var secondDigit = false;
        var i = 0;

        for (i = aux.length - 1; i >= 0; i--) {
            var digit = aux[i];
            var valor = getNumberFromChar(digit);

            if (secondDigit) {
                valor *= 2;
                if (valor > 9) {
                    valor -= 9;
                }
            }
            sum += valor;
            secondDigit = !secondDigit;
        }
        return (sum % 10) == 0;
    }

    function getNumberFromChar(char) {
        var res = 0;

        if (isNaN(char)) {
            switch (char.toUpperCase()) {
                case 'A': res = 10; break;
                case 'B': res = 11; break;
                case 'C': res = 12; break;
                case 'D': res = 13; break;
                case 'E': res = 14; break;
                case 'F': res = 15; break;
                case 'G': res = 16; break;
                case 'H': res = 17; break;
                case 'I': res = 18; break;
                case 'J': res = 19; break;
                case 'K': res = 20; break;
                case 'L': res = 21; break;
                case 'M': res = 22; break;
                case 'N': res = 23; break;
                case 'O': res = 24; break;
                case 'P': res = 25; break;
                case 'Q': res = 26; break;
                case 'R': res = 27; break;
                case 'S': res = 28; break;
                case 'T': res = 29; break;
                case 'U': res = 30; break;
                case 'V': res = 31; break;
                case 'W': res = 32; break;
                case 'X': res = 33; break;
                case 'Y': res = 34; break;
                case 'Z': res = 35; break;
                default: res = 0;
            }
        }
        else {
            res = parseInt(char);
        }

        return res;
    }


    //mostrar formato do CC caso seleccionar esse
    function mostrarPlaceholderFormatoCCouBI(docType) {
        $("#NDIWarning").text("");
        $("#NDIWarning").hide();
        if (docType == 4) {
            $("#NDI").prop("placeholder", "DDDDDDDD-DCCD");
        }
        else if (docType == 2) {
            $("#NDI").prop("placeholder", "DDDDDDDD-D");
        }
        else {
            $('#NDI').prop('placeholder', "");
        }
    }

    //validar email
    function regexEmail(email) {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(email).toLowerCase());
    }

    function validarEmail() {
        var email = $("#Email").val();
        if (email !== "") {

            if (regexEmail(email)) {
                $("#EmailWarning").hide();
                return true;
            }
            else {
                $("#EmailWarning").text("Email com formato inválido");
                $("#EmailWarning").show();
                return false;
            }
        }
        else {
            if ($("#EmailWarning").is(":visible")) {
                $("#EmailWarning").hide();
            }
            return true;
        }
    }

    function verificarEmail() {
        var email = $("#Email").val();
        var emailExists = true;
        var valid = true;

        if (email !== "") {
            var check = $.ajax({
                url: '@Url.Action("checkEmail", "User")',
                data: {
                    email: email
                },
                type: "POST",
                success: function (exists) {
                    if (exists === "True") {
                        $("#EmailWarning").text("Este email já se encontra em uso");
                        $("#EmailWarning").show();
                        emailExists = false;
                    }
                },
                error: function (e) { console.log(e); }
            });
        }
        check.done(function () {
            if (emailExists) {
                valid = true;
            } else {
                valid = false;
            }
        });
        return valid;
    }

    function validarIdd() {
        var isMil = $('#user_Militar').is(':checked');
        var dtNasc = new Date($('#DtNasc').val());
        var idd = new Date().getFullYear() - dtNasc.getFullYear();
        
        if (dtNasc >= new Date()) {
            $("#DtNascWarning").text("Data de nascimento não pode ser no futuro");
            $("#DtNascWarning").show();
            return false;
        }
        else if (!isMil && idd >= 22) {
            $("#DtNascWarning").text("Tem de ter, no máximo, 22 anos para fazer a candidatura");
            $("#DtNascWarning").show();
            return false;
        }
        else if (isMil && idd >= 24) {
            $("#DtNascWarning").text("Tem de ter, no máximo, 24 anos para fazer a candidatura");
            $("#DtNascWarning").show();
            return false;
        }
        else {
            $("#DtNascWarning").text("");
            $("#DtNascWarning").hide();
            return true;
        }
    }

    function validarDocValidade() {
        var docdate = new Date($('#DocVali').val());
        var today = new Date();
        if (docdate <= today) {
            $("#DocValiWarning").text("O documento expirou");
            $("#DocValiWarning").show();
            return false;
        }
        else {
            $("#DocValiWarning").text("");
            $("#DocValiWarning").hide();
            return true;
        }
    }

    /* Formulario */
    function validarNIF(nif) {

        if (!['1', '2', '3', '5', '6', '8'].includes(nif.substr(0, 1)) && !['45', '70', '71', '72', '77', '79', '90', '91', '98', '99'].includes(nif.substr(0, 2)))
        {
            $("#NIFWarning").text("Número de Identificação Fiscal inválido.");
            $("#NIFWarning").show();
            return false;
        }
   
        let total = nif[0] * 9 + nif[1] * 8 + nif[2] * 7 + nif[3] * 6 + nif[4] * 5 + nif[5] * 4 + nif[6] * 3 + nif[7] * 2;

        let modulo11 = total - parseInt(total / 11) * 11;
        let comparador = modulo11 == 1 || modulo11 == 0 ? 0 : 11 - modulo11;
        if (nif[8] == comparador) {
            $("#NIFWarning").text("");
            $("#NIFWarning").hide();
            return true;
        }
        else {
            $("#NIFWarning").text("Número de Identificação Fiscal inválido.");
            $("#NIFWarning").show();
            return false;
        }
    }

    //limita 4 digitos
    function limitarCodPostal4Dig() {
        var cp4 = $("#CodigoPostal4Dig").val();
        if (cp4.length <= "4") {
            $("#CPWarning").hide();
            return true;
        }
        else {
            $("#CPWarning").text("Código postal inválido");
            $("#CPWarning").show();
            return false;
        }
    }

    //limita 3 digitos
    function limitarCodPostal3Dig() {
        var cp3 = $("#CodigoPostal3Dig").val();
        if (cp3.length <= "3") {
            $("#CPWarning").hide();
            return true;
        }
        else {
            $("#CPWarning").text("Código postal inválido");
            $("#CPWarning").show();
            return false;
        }
    }

    function checkNational() {
        var nac = $("#Nacionalidade").val();
        if (nac === "PT") {
            $("#NIF").prop('required', true);
            $("label[for='NIF']").addClass('required');
            $("#RepFinNIF").prop('required', true);
            $("label[for='RepFinNIF']").addClass('required');
            $('#DistritoNatural').prop('disabled', false);
            validarNIF($("#NIF").val());
        }
        else {
            $("#NIF").prop('required', false);
            $("label[for='NIF']").removeClass('required');
            $("#RepFinNIF").prop('required', false);
            $("label[for='RepFinNIF']").removeClass('required');
            $('#DistritoNatural').prop('disabled', true);
            $("#NIFWarning").text("");
            $("#NIFWarning").hide();
        }
    }

    function validacaoFinal() {
        var valid = true;

        if (!validarTipoDocId()) {
            valid = false;
        }

        if (!validarEmail()) {
            valid = false;
        }

        if (!verificarEmail()) {
            valid = false;
        }
        if (!validarIdd()) {
            valid = false;
        }

        if (!valid) {
            event.preventDefault();
            return false;
        }
    }
});