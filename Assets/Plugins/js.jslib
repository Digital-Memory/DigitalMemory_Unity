mergeInto(LibraryManager.library, {

  EnterTime: function(time) {
        window.alert("enter time: " + time);
    },

  EnterCloseup: function(closeUp) {
        window.alert("enter CloseUp: " + closeUp);
    },
  LeaveCloseup: function() {
        window.alert("leave CloseUp.");
    },


});