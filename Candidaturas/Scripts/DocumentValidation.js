$(document).ready(function () {

    $("#NDI").blur(function () {
        var number = $("#NDI").val();
        
        if (number !== "") {
            validarTipoDocId(number);
        }        
        mostrarPlaceholderFormatoCCouBI();
    });

    $("#Email").blur(function () {
        validarEmail();
        verificarEmail();
    });

    $("#TipoDocID").change(function () {
        var number = $("#NDI").val();
        if (number !== "") {
            validarTipoDocId(number);
        } 
        mostrarPlaceholderFormatoCCouBI();
    });
    $("#DataNascimento").change(function () {
        validarIdd();
    });

    function validarTipoDocId(number) {
        var chosenValue = $("#TipoDocID").val();

        if (chosenValue === "Cartão de Cidadão") {
            if (validarCC(number) && validarDigitosCC(number)) {
                $("#NDIWarning").hide();
                return true;
            }
            else {
                $("#NDIWarning").text("Cartão de Cidadão inválido");
                $("#NDIWarning").show();
                return false;
            }
        }

        if (chosenValue === "Bilhete de Identidade") {
            if (validarBI(number) && validarDigitosBI(number)) {
                $("#NDIWarning").hide();
                return true;
            }
            else {
                $("#NDIWarning").text("Bilhete de Identidade inválido");
                $("#NDIWarning").show();
                return false;
            }
        }
    
        if ($("#NDIWarning").is(":visible")) {
            $("#NDIWarning").hide();
        }
            return true;
        }


    function validarCC(value) {
        var re = new RegExp("^[0-9]{8}[ -]*[0-9][A-Za-z]{2}[0-9]$");
        return re.test(value);
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

        if (res === 0 || res === 1) {
            res = 0;
        }
        else {
            res = 11 - res;
        }

        return res === lastDigit;
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

        return (sum % 10) === 0;
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

    function validarBI(value)
    {
        var re = new RegExp("^[0-9]{8}[ -]*[0-9]$");
        return re.test(value);
    }

    //mostrar formato do CC caso seleccionar esse
    function mostrarPlaceholderFormatoCCouBI() {
        var chosenValue = $("#TipoDocID").val();

        if (chosenValue === "Cartão de Cidadão") {
            $("#NDI").prop("placeholder", "DDDDDDDD-DCCD");
        }
        else if (chosenValue === "Bilhete de Identidade") {
            $("#NDI").prop("placeholder", "DDDDDDDD-D");
        }
        else {
            $('#NDI').prop('placeholder',"");
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

    //muda a label do switch
    $('#Militar').click(function () {
        changeLabel();
    });

    $(document).ready(function () {
        changeLabel();
    });

    function changeLabel() {
        validarIdd();
        if (!$("#Militar").is(':checked')) {
            $("#switchLabel").text("Não");
        }
        else {
            $("#switchLabel").text("Sim");
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
                $("#EmailWarning").text("Email com formato inválido.");
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
        
        if (email != "") {
            var check = $.ajax({
                url: '@Url.Action("checkEmail", "User")',
                data: {
                    email: email
                },
                type: "POST",
                success: function (exists) {
                    if (exists == "True") {
                        $("#EmailWarning").text("Este email já se encontra em uso.");
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
        var isMil = $('#Militar').is(':checked');
        var dtNasc = new Date($('#DataNascimento').val());
        var idd = new Date().getFullYear() - dtNasc.getFullYear();

        if (dtNasc >= new Date()) {
            $("#DtNascWarning").text("Data de nascimento não pode ser no futuro.");
            $("#DtNascWarning").show();
            return false;
        }
        else if (!isMil && idd >= 22) {
            $("#DtNascWarning").text("Tem de ter 22 anos para fazer a candidatura.");
            $("#DtNascWarning").show();
            return false;
        }
        else if (isMil && idd >= 24) {
            $("#DtNascWarning").text("Tem de ter 24 anos para fazer a candidatura.");
            $("#DtNascWarning").show();
            return false;
        }
        else {
            $("#DtNascWarning").text("");
            $("#DtNascWarning").hide();
            return true;
        }
    }

});