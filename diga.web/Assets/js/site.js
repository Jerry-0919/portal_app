window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    if (document.body.scrollTop > 30 || document.documentElement.scrollTop > 30) {
        document.getElementById("second_navbar").style.marginTop = "0px";
        document.getElementById("second_logo").style.display = "block";
    } else {
        document.getElementById("second_navbar").style.marginTop = "64px";
        document.getElementById("second_logo").style.display = "none";
    }
}
$('#navbarNav2').on('show.bs.collapse', function () {
    $(this).append($('#navbarNav').html());
    $(this).append($('#navbarNavSocial').html());
    $('#navbarNav2 ul').last().addClass('navbar-nav');
    $('#navbarNav2 ul').removeClass('text-center nav-justified');
    $('#navbarNav2 ul li a').removeClass("text-white");
    $('#mobile_opps').attr("href", "#");
    $('#mobile_opps').attr("data-toggle", "dropdown");
});
$('#navbarNav2').on('hidden.bs.collapse', function () {
    $('#navbarNav2 ul:last-child').remove();
    $('#navbarNav2 ul:last-child').remove();
    $('#mobile_opps').attr("href", "/modules/index");
    $('#mobile_opps').removeAttr("data-toggle");
});
$(window).on('resize', function () {
    if (window.innerWidth > 768) { $('#navbarNav2').collapse('hide'); }
});