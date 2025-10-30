mergeInto(LibraryManager.library, {
  FocusInputField: function (unityObjNamePtr, isPassword) {
    var unityObjName = UTF8ToString(unityObjNamePtr); // <-- Dùng biến này
    var input = document.getElementById("mobileKeyboardInput");

    var canvas = document.getElementById("#unity-canvas") || document.querySelector("#unity-canvas");
    // Khởi tạo input nếu chưa có
    if (!input) {
      input = document.createElement("input");
      input.id = "mobileKeyboardInput";
      input.style.position = "absolute";
      input.style.borderRadius = "10px";
      input.style.zIndex = 1000;
      input.style.fontSize = "30px";
      input.style.transition = "opacity 0.2s";
      input.style.pointerEvents = "auto";
      document.body.appendChild(input);

      // Thêm Event Listener vào document
      // Khi click, nó sẽ kiểm tra xem click có phải trên input không.
      var hideInputHandler = function (event) {
        const currentInput = document.getElementById("mobileKeyboardInput");

        // Kiểm tra input có tồn tại, đang hiện (opacity 1) và click không phải trên input
        if (
          currentInput &&
          currentInput.style.opacity === "1" &&
          event.target !== currentInput &&
          !currentInput.contains(event.target)
        ) {
          currentInput.blur();
          currentInput.style.opacity = "0";
          currentInput.style.pointerEvents = "none";
          if (canvas) {
            canvas.style.pointerEvents = "auto";

            console.log("thuc hien blur input va gui message den unity");
          }
          SendMessage(unityObjName, "OnEndEditFromJs", currentInput.value);
          // Bỏ focus và ẩn input

          // Gửi tin nhắn đến Unity thông báo kết thúc chỉnh sửa
          // Dùng unityObjName đã lưu trữ trong Closure scope của listener
          // **SỬA LỖI currentOwner TẠI ĐÂY**
        }
      };

      document.addEventListener("mousedown", hideInputHandler, true);
      document.addEventListener("touchstart", hideInputHandler, true);
    }

    // Cập nhật vị trí và thuộc tính của input
    const vw = window.innerWidth;
    const vh = window.innerHeight;
    const iw = 300; // chiều rộng ô input
    const ih = 50 // chiều cao ô input
    input.style.width = iw + "px";
    input.style.height = ih + "px";
    input.style.left = vw / 2 - iw / 2 + "px";
    input.style.top = vh / 2 - ih / 2 + "px";

    // Xóa các event handler cũ trước khi gán lại
    input.oninput = null;
    input.onkeydown = null;

    input.type = isPassword ? "password" : "text";
    input.value = "";
    input.style.display = "block";
    input.style.opacity = "1";
    input.style.pointerEvents = "auto";
    if (canvas) {
      canvas.style.pointerEvents = "auto";
    }

    input.focus();

    // Gán lại event handler oninput
    input.oninput = function () {
      // **SỬA LỖI currentOwner TẠI ĐÂY**
      SendMessage(unityObjName, "OnKeyboardValueChanged", input.value);
    };

    // Gán lại event handler onkeydown (cho Enter)
    input.onkeydown = function (e) {
      if (e.key === "Enter" || e.keyCode === 13) {
        e.preventDefault();
        SendMessage(unityObjName, "OnEndEditFromJs", input.value);
        input.blur();
        input.style.opacity = "0";
        input.style.pointerEvents = "none";

        if (canvas) {
          canvas.style.pointerEvents = "auto";
        }
      }
    };

    input.addEventListener("blur", function (e) {
      // Delay một chút để tránh conflict với các event khác
      setTimeout(function () {
        var currentInput = document.getElementById("mobileKeyboardInput");
        if (currentInput && currentInput.style.opacity === "1") {
          currentInput.style.opacity = "0";
          currentInput.style.pointerEvents = "none";

          if (canvas) {
            canvas.style.pointerEvents = "auto";
          }
          SendMessage(targetObj, "OnEndEditFromJs", currentInput.value);
        }
      }, 100);
    });
    input.setAttribute("enterkeyhint", "done");
  },

  HideInputField: function () {
    var input = document.getElementById("mobileKeyboardInput");
    if (input) {
      input.blur();
      input.style.opacity = "0";
      input.style.pointerEvents = "none";
      if (canvas) {
        canvas.style.pointerEvents = "auto";
      }
    }
  },

  // ✅ Bổ sung hàm IsMobileDevice
  IsMobileDevice: function () {
    var ua = navigator.userAgent || navigator.vendor || window.opera;
    if (/android/i.test(ua)) return true;
    if (/iPhone|iPad|iPod/i.test(ua)) return true;
    if (/Windows Phone/i.test(ua)) return true;
    return false;
  },
});
