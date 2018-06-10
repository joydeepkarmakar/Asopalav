function mobileCheck() {
    var winWidth = $(window).width();
    if (winWidth <= 768) {
        $("#sidebar").after($("#body .pagination:first"))
    } else {
        $(".products-wrap").before($("#body .pagination:first"))
    }
}

$(document).ready(function () {
    //$("input[type=checkbox]").crfi();
    //$("select").crfs();
    $("#slider ul").bxSlider({
        controls: false,
        auto: true,
        mode: 'fade',
        preventDefaultSwipeX: false
    });
    $(".last-products .products").bxSlider({
        pager: false,
        minSlides: 1,
        maxSlides: 5,
        slideWidth: 235,
        slideMargin: 0
    });
    $(".tabs .nav a").click(function () {
        var container = $(this).parentsUntil(".tabs").parent();
        if (!$(this).parent().hasClass("active")) {
            container.find(".nav .active").removeClass("active")
            $(this).parent().addClass("active")
            container.find(".tab-content").hide()
            $($(this).attr("href")).show();
        };
        return false;
    });
    $("#price-range").slider({
        range: true,
        min: 0,
        max: 5000,
        values: [500, 3500],
        slide: function (event, ui) {
            $(".ui-slider-handle:first").html("<span>$" + ui.values[0] + "</span>");
            $(".ui-slider-handle:last").html("<span>$" + ui.values[1] + "</span>");
        }
    });
    $(".ui-slider-handle:first").html("<span>$" + $("#price-range").slider("values", 0) + "</span>");
    $(".ui-slider-handle:last").html("<span>$" + $("#price-range").slider("values", 1) + "</span>");
    $("#menu .trigger").click(function () {
        $(this).toggleClass("active").next().toggleClass("active")
    });

    mobileCheck();
    $(window).resize(function () {
        mobileCheck();
    });

    /*Gifts Submenu*/
    var isGiftsSubMenuVisible = false;
    var isGiftsMenuClicked = false;

    $(document).on('click', '#mnuGifts', function () {
        isGiftsMenuClicked = true;
        if (!$("#ulGiftsSubmenu").is(":visible")) {
            $('#ulGiftsSubmenu').slideDown();
            isGiftsSubMenuVisible = true;
        }
        else {
            $('#ulGiftsSubmenu').slideUp();
            isGiftsSubMenuVisible = false;
        }
    });

    $(document).click(function () {
        if ((!isGiftsMenuClicked) && (isGiftsSubMenuVisible)) {
            $('#ulGiftsSubmenu').slideUp();
        }
        else {
            if (!isGiftsSubMenuVisible)
                $('#ulGiftsSubmenu').slideUp();
        }
        isGiftsMenuClicked = false;

    });

    /*Datepicker*/
    $(".ui-date-picker").datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true,
        autoclose: true,
        yearRange: '1950:' + new Date().getFullYear().toString()
    });

    /*Dropzone for Images*/
    $("div#divDropImages").dropzone({
        url: "/Admin/Product/UploadImage",
        init: function () {
            this.on("addedfile", function (file) {
                // Create the remove button
                var removeButton = Dropzone.createElement('<button data-dz-remove="" class="btn btn-danger delete"><i class="glyphicon glyphicon-trash"></i><span>Delete</span></button>');

                // Capture the Dropzone instance as closure.
                var _this = this;

                // Listen to the click event
                removeButton.addEventListener("click", function (e) {
                    // Make sure the button click doesn't submit the form:
                    e.preventDefault();
                    e.stopPropagation();
                    // Remove the file preview.
                    _this.removeFile(file);
                    // If you want to the delete the file on the server as well,
                    // you can do the AJAX request here.
                });

                // Add the button to the file preview element.
                file.previewElement.appendChild(removeButton);
            });
        }
    });

    /*Success & Error Message After Product Save*/
    /*
     //Your code
        alert('@TempData["AlertMessage"]');

    // Correct code
        alert('@(TempData["AlertMessage"])');
     */
    if ('@(TempData["isSaved"])' == "true")
        toastr.success('@(TempData["Msg"])');
    //else
    //    toastr.error('@(TempData["Msg"])');
    //alert('@TempData["Msg"]');
});