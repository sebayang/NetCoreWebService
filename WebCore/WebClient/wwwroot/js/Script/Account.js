﻿﻿function Login() {
    debugger;
	var validate = new Object();
	validate.Email = $('#Email').val();
	validate.Password = $('#Password').val();
	$.ajax({
		type: 'POST',
		url: "/validate/",
		cache: false,
		dataType: "JSON",
		data: validate
	}).then((result) => { 
        debugger;
		if (result.status === true) {
			window.location.href = "/home";
        } else if (result.status === "everif") {
            window.location.href = "/everif";
        }
        else {            
			$.notify({ 
                icon: 'fas fa-alarm-clock',
				title: 'Notification',
				message: result.msg,
			}, { 
				element: 'body',
				type: "danger",
				allow_dismiss: true,
				placement: {
					from: "top",
					align: "center"
				},
				timer: 1000,
				delay: 5000,
				animate: {
					enter: 'animated fadeInDown',
					exit: 'animated fadeOutUp'
				},
                icon_type: 'class',
                template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                    '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                    '<span data-notify="icon"></span> ' +
                    '<span data-notify="title">{1}</span> ' +
                    '<span data-notify="message">{2}</span>' +
                    '<div class="progress" data-notify="progressbar">' +
                    '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                    '</div>' +
                    '<a href="{3}" target="{4}" data-notify="url"></a>' +
                    '</div>' 
            });
		}
	})
}


function Register() {
    if ($('#confirmPass').val() === $('#Pass').val()) {
        debugger;
        var auth = new Object();
        auth.UserName = $('#Uname').val();
        auth.Email = $('#Email').val();
        auth.Password = $('#Pass').val();
        auth.Phone = $('#Phone').val();
        $.ajax({
            type: 'POST',
            url: "/validate/",
            cache: false,
            dataType: "JSON",
            data: auth
        }).then((result) => {
            debugger;
            if (result.status === "everif") { 
                window.location.href = "/everif";
            } else {
                $.notify({
                    // options
                    icon: 'fas fa-alarm-clock',
                    title: 'Notification',
                    message: result.msg,
                }, {
                    // settings
                    element: 'body',
                    type: "danger",
                    allow_dismiss: true,
                    placement: {
                        from: "top",
                        align: "center"
                    },
                    timer: 1000,
                    delay: 5000,
                    animate: {
                        enter: 'animated fadeInDown',
                        exit: 'animated fadeOutUp'
                    },
                    icon_type: 'class',
                    template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                        '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                        '<span data-notify="icon"></span> ' +
                        '<span data-notify="title">{1}</span> ' +
                        '<span data-notify="message">{2}</span>' +
                        '<div class="progress" data-notify="progressbar">' +
                        '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                        '</div>' +
                        '<a href="{3}" target="{4}" data-notify="url"></a>' +
                        '</div>' 
                });
            }
        })
    } else {
        $.notify({
            // options
            icon: 'fas fa-alarm-clock',
            title: 'Notification',
            message: 'Password Not Same',
        }, {
            // settings
            element: 'body',
            type: "warning",
            allow_dismiss: true,
            placement: {
                from: "top",
                align: "center"
            },
            timer: 1000,
            delay: 5000,
            animate: {
                enter: 'animated fadeInDown',
                exit: 'animated fadeOutUp'
            },
            icon_type: 'class',
            template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                '<span data-notify="icon"></span> ' +
                '<span data-notify="title">{1}</span> ' +
                '<span data-notify="message">{2}</span>' +
                '<div class="progress" data-notify="progressbar">' +
                '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                '</div>' +
                '<a href="{3}" target="{4}" data-notify="url"></a>' +
                '</div>' 
        });
    }
} 

function everif() {
    debugger; 
    var code = $('#Code').val(); 
    $.ajax({
        type: 'POST',
        url: "/verifemail/",
        cache: false,
        dataType: "JSON",
        data: { code : code }
    }).then((result) => {
        debugger;
        if (result.status === true) {
            window.location.href = "/home";
        } else {
            $.notify({
                icon: 'fas fa-alarm-clock',
                title: 'Notification',
                message: result.msg,
            }, {
                    element: 'body',
                    type: "danger",
                    allow_dismiss: true,
                    placement: {
                        from: "top",
                        align: "center"
                    },
                    timer: 1000,
                    delay: 5000,
                    animate: {
                        enter: 'animated fadeInDown',
                        exit: 'animated fadeOutUp'
                    },
                    icon_type: 'class',
                    template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                        '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                        '<span data-notify="icon"></span> ' +
                        '<span data-notify="title">{1}</span> ' +
                        '<span data-notify="message">{2}</span>' +
                        '<div class="progress" data-notify="progressbar">' +
                        '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                        '</div>' +
                        '<a href="{3}" target="{4}" data-notify="url"></a>' +
                        '</div>'
                });
        }
    })
}