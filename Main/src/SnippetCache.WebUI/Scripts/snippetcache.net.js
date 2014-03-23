​​​​function topBar(​​​message) {
    $("<div />", { 'class': 'topbar', text: message }).hide().prependTo("body")
        .slideDown('fast').delay(10000).slideUp(function() { $(this).remove(); });
}