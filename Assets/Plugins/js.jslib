mergeInto(LibraryManager.library, {

  EnterTime: function(time) {
    const message = {
      type: 'EnterTime',
      data: {
        time: time
      }
    }
    window.postMessage(message, '*')
  },
  EnterCloseup: function(closeup) {
    const message = {
      type: 'EnterCloseup',
      data: {
        closeup: closeUp
      }
    }
    window.postMessage(message, '*')
  },
  LeaveCloseup: function(closeup) {
    const message = {
      type: 'LeaveCloseup',
      data: {
        closeup: closeUp
      }
    }
    window.postMessage(message, '*')
  },
});