// Custom JavaScript for Elearn

// Wait for the DOM to be ready
$(function () {
    // Initialize AdminLTE components
    if (typeof $.fn.overlayScrollbars !== 'undefined') {
        $('.sidebar').overlayScrollbars({
            className: 'os-theme-light',
            sizeAutoCapable: true,
            scrollbars: {
                autoHide: 'leave',
                clickScrolling: true
            }
        });
    }

    // Initialize tooltips
    $('[data-toggle="tooltip"]').tooltip();

    // Initialize popovers
    $('[data-toggle="popover"]').popover();

    // Handle card widget collapse
    $('[data-card-widget="collapse"]').on('click', function () {
        var $card = $(this).closest('.card');
        $card.find('.card-body, .card-footer').slideToggle();
        $(this).find('i').toggleClass('fa-minus fa-plus');
    });

    // Handle card widget remove
    $('[data-card-widget="remove"]').on('click', function () {
        $(this).closest('.card').fadeOut();
    });

    // Handle sidebar toggle
    $('[data-widget="pushmenu"]').on('click', function (e) {
        e.preventDefault();
        $('body').toggleClass('sidebar-collapse');

        // Save sidebar state to localStorage
        if ($('body').hasClass('sidebar-collapse')) {
            localStorage.setItem('sidebar-collapse', 'true');
        } else {
            localStorage.setItem('sidebar-collapse', 'false');
        }
    });

    // Check sidebar state on page load
    if (localStorage.getItem('sidebar-collapse') === 'true') {
        $('body').addClass('sidebar-collapse');
    } else {
        $('body').removeClass('sidebar-collapse');
    }

    // Handle fullscreen toggle
    $('[data-widget="fullscreen"]').on('click', function () {
        $('body').toggleClass('layout-fullscreen');
    });

    // Handle control sidebar toggle
    $('[data-widget="control-sidebar"]').on('click', function () {
        $('body').toggleClass('control-sidebar-slide-open');
    });

    // Handle navbar search toggle
    $('[data-widget="navbar-search"]').on('click', function () {
        $('.navbar-search-block').toggleClass('navbar-search-open');
        if ($('.navbar-search-block').hasClass('navbar-search-open')) {
            $('.navbar-search-block input').focus();
        }
    });

    // Handle sidebar search
    $('.form-control-sidebar').on('keyup', function () {
        var term = $(this).val().trim().toLowerCase();
        if (term.length > 0) {
            $('.nav-sidebar .nav-link').each(function () {
                var text = $(this).text().trim().toLowerCase();
                if (text.indexOf(term) > -1) {
                    $(this).closest('.nav-item').show();
                } else {
                    $(this).closest('.nav-item').hide();
                }
            });
        } else {
            $('.nav-sidebar .nav-item').show();
        }
    });

    // Add active class to nav items based on current page
    var currentUrl = window.location.pathname;
    $('.nav-sidebar .nav-link').each(function () {
        var linkUrl = $(this).attr('href');
        if (linkUrl && currentUrl.indexOf(linkUrl) > -1) {
            $(this).addClass('active');
            $(this).closest('.nav-treeview').prev().addClass('active');
            $(this).closest('.nav-treeview').parent().addClass('menu-open');
        }
    });
});
