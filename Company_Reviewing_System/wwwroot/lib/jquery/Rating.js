$.fn.stars = function () {
    return this.each(function () {
        // Get the value
        var val = parseFloat($(this).html());
        // Make sure that the value is in 0 - 5 range, multiply to get width
        var size = Math.max(0, (Math.min(5, val))) * 36.5;
        // Create stars holder
        var $span = $('<span> </span>').width(size);
        // Replace the numerical value with stars
        $(this).empty().append($span);
    });
}

$(function () {
    console.log("Calling stars()");
    $('.results-content span.stars').stars();
});