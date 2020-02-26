// Example 1
$('.pane-hScroll').scroll(function() {
  $('.pane-vScroll').width($('.pane-hScroll').width() + $('.pane-hScroll').scrollLeft());
});



// Example 2
$('.pane--table2').scroll(function() {
  $('.pane--table2 table').width($('.pane--table2').width() + $('.pane--table2').scrollLeft());
});

