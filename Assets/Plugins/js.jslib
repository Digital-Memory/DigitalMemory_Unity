mergeInto(LibraryManager.library, {

  EnterTime: function(time) {
        window.alert("enter time: " + time);
    },

  EnterCloseup: function(id) {
        window.alert("enter CloseUp: " + id);
    },
  LeaveCloseup: function() {
        window.alert("leave CloseUp.");
    },


});