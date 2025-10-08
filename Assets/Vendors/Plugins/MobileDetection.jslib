mergeInto(LibraryManager.library, {
  FocusInputField: function (unityObjNamePtr, isPassword) {
    var unityObjName = UTF8ToString(unityObjNamePtr);

    var input = document.getElementById("mobileKeyboardInput");
    if (!input) {
      input = document.createElement("input");
      input.id = "mobileKeyboardInput";
      input.style.position = "absolute";
      input.style.top = "50%";
      input.style.left = "50%";
      input.style.zIndex = 1000;
      input.style.fontSize = "32px";
      document.body.appendChild(input);
    }

    input.oninput = null;
    input.onkeydown = null;
    input.type = (isPassword ? "password" : "text");

    input.value = "";
    //input.setSelectionRange(input.value.length, input.value.length);
    input.select();
    input.style.opacity = "1";
    input.focus();

    input.oninput = function () {
      SendMessage(unityObjName, "OnKeyboardValueChanged", input.value);
    };

    input.onkeydown = function (e) {
      if (e.key === "Enter") {
        SendMessage(unityObjName, "OnEndEditFromJs", input.value);
        input.blur();
        input.style.opacity = "0";
      }
    };
  },

  HideInputField: function () {
    var input = document.getElementById("mobileKeyboardInput");
    if (input) {
      input.blur();
      input.style.opacity = "0";
    }
  },

  // ✅ Bổ sung hàm IsMobileDevice
  IsMobileDevice: function () {
    var ua = navigator.userAgent || navigator.vendor || window.opera;
    if (/android/i.test(ua)) return true;
    if (/iPhone|iPad|iPod/i.test(ua)) return true;
    if (/Windows Phone/i.test(ua)) return true;
    return false;
  }
});
