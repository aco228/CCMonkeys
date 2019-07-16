var isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
$('body').addClass(isMobile ? 'mobile' : 'pc');

var date = new Date();
var months = ["January","February","March","April","May","June","July","August","September","October","November","December"];
document.getElementById("month").innerHTML = months[date.getMonth()];
document.getElementById("day").innerHTML = date.getDate();
document.getElementById("year").innerHTML = date.getFullYear();


function showPage() {
	document.getElementById('popUp').style.cssText = 'display: none;';
    if (isMobile) {
    setTimeout(function () {
        $('html, body').animate({
            scrollTop: $(".reference").offset().top - 0
        }, 450);
    }, 2600)
}
}

var currentQuestion = 1;
function showQuestion(){
	currentQuestion++;
	var questionNumberToHide = currentQuestion-1;
    $('#question' + questionNumberToHide).fadeOut(500);
    $('#question' + currentQuestion).delay(500).fadeIn().addClass('active');
}

function showProgressBar(){
	$('#question7').fadeOut(500);
	$('.progress-section').delay(500).fadeIn();
	move();
}


function move() {
    var elem = document.getElementById("pr-test"); 
    var width = 1;
    var id = setInterval(frame, 60);
    function frame() {
        if (width >= 100) {
            clearInterval(id);
        } else {
            width++; 
            elem.style.width = width + '%'; 
        }
    }

    setTimeout(function(){ 
    	$('.progress-section').fadeOut();
    	$('#dear-box').fadeOut();
    	$('#thank-you-box').delay(500).fadeIn();
        $('.last-section').delay(500).fadeIn();
        $('.sold').addClass("sold-1");
        $('.img-sold').addClass("img-sold-1");
        $('.sold button').attr("disabled", true);
    }, 6000);
    setTimeout(function(){ 
        $('html, body').animate({
            scrollTop: $(".last-section").offset().top
        },1000);
    }, 7000);

}