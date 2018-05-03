/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, {
/******/ 				configurable: false,
/******/ 				enumerable: true,
/******/ 				get: getter
/******/ 			});
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = "./Scripts/wp-sw.ts");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./Scripts/wp-sw.ts":
/*!**************************!*\
  !*** ./Scripts/wp-sw.ts ***!
  \**************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("var WebPushServiceWorker;\r\n(function (WebPushServiceWorker) {\r\n    self.addEventListener('push', function (event) {\r\n        if (event.data) {\r\n            const data = event.data.json();\r\n            const notification = data.notification || {};\r\n            const title = getTitle(notification);\r\n            const options = getOptions(notification);\r\n            var callback = getCallback(data);\r\n            if (callback !== null) {\r\n                const promises = Promise.all([\r\n                    callback,\r\n                    self.registration.showNotification(title, options)\r\n                ]);\r\n                event.waitUntil(promises);\r\n            }\r\n            else {\r\n                event.waitUntil(self.registration.showNotification(title, options));\r\n            }\r\n        }\r\n        else { }\r\n    });\r\n    self.addEventListener('notificationclick', function (event) {\r\n        const clickedNotification = event.notification;\r\n        clickedNotification.close();\r\n        const url = clickedNotification.data && clickedNotification.data.url\r\n            ? clickedNotification.data.url\r\n            : '/';\r\n        const urlToOpen = new URL(url, self.location.origin).href;\r\n        const promiseChain = clients.matchAll({\r\n            type: 'window',\r\n            includeUncontrolled: true\r\n        })\r\n            .then(windowClients => {\r\n            var matchingClient = null;\r\n            for (var i = 0; i < windowClients.length; i++) {\r\n                var windowClient = windowClients[i];\r\n                if (windowClient.url === urlToOpen) {\r\n                    matchingClient = windowClient;\r\n                    break;\r\n                }\r\n            }\r\n            if (matchingClient) {\r\n                return matchingClient.focus();\r\n            }\r\n            else {\r\n                return clients.openWindow(urlToOpen);\r\n            }\r\n        });\r\n        event.waitUntil(promiseChain);\r\n    });\r\n    function getTitle(data) {\r\n        return data.title || \"\";\r\n    }\r\n    function getOptions(data) {\r\n        const actions = data.actions && Array.isArray(data.actions) && data.actions.length > 0\r\n            ? data.actions.slice(0, Math.min(Notification.maxActions, data.actions.length))\r\n            : [];\r\n        const options = {\r\n            body: data.message || null,\r\n            icon: data.iconUrl || null,\r\n            image: data.imageUrl || null,\r\n            actions: actions,\r\n            data: {\r\n                url: data.url || null,\r\n            }\r\n        };\r\n        return options;\r\n    }\r\n    function getCallback(data) {\r\n        if (!data.callbackUrl)\r\n            return null;\r\n        return fetch(data.callbackUrl, {\r\n            headers: {\r\n                'Accept': 'application/json',\r\n                'Content-Type': 'application/json'\r\n            },\r\n            method: 'GET',\r\n        });\r\n    }\r\n})(WebPushServiceWorker || (WebPushServiceWorker = {}));\r\n\n\n//# sourceURL=webpack:///./Scripts/wp-sw.ts?");

/***/ })

/******/ });