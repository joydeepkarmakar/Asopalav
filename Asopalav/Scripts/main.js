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

    /*Buy Now button in Product Details*/
    $('[data-toggle="tooltip"]').tooltip();
    $('.add-to-cart').click(function () {
        $('#noCartItems').hide();
        $('#hasCartItems').show();
    });

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

    $(document).on('click', '#mnuSilverware', function () {
        isSilverwareMenuClicked = true;
        if (!$("#ulSilverwareSubmenu").is(":visible")) {
            $('#ulSilverwareSubmenu').slideDown();
            isSilverwareSubMenuVisible = true;
        }
        else {
            $('#ulSilverwareSubmenu').slideUp();
            isSilverwareSubMenuVisible = false;
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

    $('.row ul li').hover(
        function () {
            //we get our current filename and use it for the src
            var linkIndex = $(this).attr("imgName");
            $(this).addClass('hover');
            $('.box img').attr('src', linkIndex + '.jpg');
        },
        function () {
            $(this).removeClass('hover');
            $('.row carousel-item img').attr('src', 'image1.jpg');
        }
    );

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

    /*Feedback Form*/
    //$(document).ready(function () {
    $('#btnSubmit').click(function () {
        if ($("#frmFeedback").valid()) {
            $('#frmFeedback').submit();
        }
        else {
            return false;
        }
    });
    $("#frmFeedback").on("submit", function (event) {
        event.preventDefault();
        $('#btnSubmit').attr('disabled', 'disabled');
        var url = $(this).attr("action");
        var formData = $(this).serialize();
        $.ajax({
            url: url,
            type: "POST",
            data: formData,
            dataType: "json",
            success: function (response) {
                if (response.IsSuccess)
                    toastr.success('Success!');
                else
                    toastr.error(response.errorMsg);
            },
            error: function (response) {
                toastr.error('Error!');
            },
            complete: function () {
                $('#btnSubmit').removeAttr('disabled');
                $("input[type=text], textarea").val("");
            }
        })
    });
    //});

    /*Product Details*/

    if ($('#ddlCurrency').val() == "USD")
        $(".dispCurrency").text("$");
    else
        $(".dispCurrency").text("₹");

    var dispPrice = $(".dispPrice").text();

    $("#ddlCurrency").change(function () {
        if ($("#ddlCurrency option:selected").text() == "USD") {
            $(".dispCurrency").text("$");
            $(".dispPrice").text(dispPrice);
        }
        else {
            $(".dispCurrency").text("₹");
            $(".dispPrice").text((dollarRate * dispPrice).toFixed(2));
        }
    });


    /*Add to Cart*/
    var productCustomization = $('.cd-customization'),
        cart = $('.cd-cart'),
        animating = false;
    initCustomization(productCustomization);

    function initCustomization(items) {
        items.each(function () {
            var actual = $(this),
                addToCartBtn = actual.find('.add-to-cart');

            //detect click on the add-to-cart button
            addToCartBtn.on('click', function () {
                if (!animating) {
                    //animate if not already animating
                    animating = true;
                    //resetCustomization(addToCartBtn);

                    addToCartBtn.addClass('is-added').find('path').eq(0).animate({
                        //draw the check icon
                        'stroke-dashoffset': 0
                    }, 300, function () {
                        setTimeout(function () {
                            updateCart(prodId);
                            addToCartBtn.removeClass('is-added').find('em').on('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function () {
                                //wait for the end of the transition to reset the check icon
                                addToCartBtn.find('path').eq(0).css('stroke-dashoffset', '19.79');
                                animating = false;
                            });

                            if ($('.no-csstransitions').length > 0) {
                                // check if browser doesn't support css transitions
                                addToCartBtn.find('path').eq(0).css('stroke-dashoffset', '19.79');
                                animating = false;
                            }
                        }, 600);
                    });
                }
            });
        });
    }

    function updateCart(prodId) {

        var url = "/CartDetails/AddToCartPartial";

        $.get(url, {
            id: prodId
        }, function (data) {
            cart.html(data);
        });

        //show counter if this is the first item added to the cart
        (!cart.hasClass('items-added')) && cart.addClass('items-added');

        var cartItems = cart.find('span'),
            text = parseInt(cartItems.text()) + 1;
        cartItems.text(text);
    }


});