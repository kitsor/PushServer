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
/******/ 	return __webpack_require__(__webpack_require__.s = "./Scripts/wps.ts");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./Scripts/services/apiClient.ts":
/*!***************************************!*\
  !*** ./Scripts/services/apiClient.ts ***!
  \***************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\r\nvar __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {\r\n    return new (P || (P = Promise))(function (resolve, reject) {\r\n        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }\r\n        function rejected(value) { try { step(generator[\"throw\"](value)); } catch (e) { reject(e); } }\r\n        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }\r\n        step((generator = generator.apply(thisArg, _arguments || [])).next());\r\n    });\r\n};\r\nObject.defineProperty(exports, \"__esModule\", { value: true });\r\nclass ApiClient {\r\n    constructor(_subscriptionsEndpoint) {\r\n        this._subscriptionsEndpoint = _subscriptionsEndpoint;\r\n    }\r\n    save(subscriptionInfo) {\r\n        return __awaiter(this, void 0, void 0, function* () {\r\n            const data = subscriptionInfo.toJSON();\r\n            const response = yield fetch(this._subscriptionsEndpoint, {\r\n                headers: {\r\n                    'Accept': 'application/json',\r\n                    'Content-Type': 'application/json'\r\n                },\r\n                method: \"POST\",\r\n                body: data\r\n            });\r\n            var contentType = response.headers.get(\"content-type\");\r\n            if (response.ok && contentType && contentType.includes(\"application/json\")) {\r\n                const json = yield response.json();\r\n                if ('errorCode' in json && json.errorCode == 0) {\r\n                    return json.data.userId;\r\n                }\r\n            }\r\n            return null;\r\n        });\r\n    }\r\n    delete(userId) {\r\n        return __awaiter(this, void 0, void 0, function* () {\r\n            const response = yield fetch(this._subscriptionsEndpoint + `/uid/${userId}`, {\r\n                headers: {\r\n                    'Accept': 'application/json',\r\n                    'Content-Type': 'application/json'\r\n                },\r\n                method: \"DELETE\"\r\n            });\r\n            var contentType = response.headers.get(\"content-type\");\r\n            if (response.ok) {\r\n                return true;\r\n            }\r\n            return false;\r\n        });\r\n    }\r\n}\r\nexports.ApiClient = ApiClient;\r\n\n\n//# sourceURL=webpack:///./Scripts/services/apiClient.ts?");

/***/ }),

/***/ "./Scripts/services/localStorage.ts":
/*!******************************************!*\
  !*** ./Scripts/services/localStorage.ts ***!
  \******************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\r\nObject.defineProperty(exports, \"__esModule\", { value: true });\r\nclass LocalStorage {\r\n    constructor(namespace = \"wpsManager\") {\r\n        this.namespace = namespace;\r\n    }\r\n    setItem(key, item) {\r\n        let items = this.getItems();\r\n        items[key] = item;\r\n        this.setItems(items);\r\n    }\r\n    getItem(key) {\r\n        let items = this.getItems();\r\n        return key in items ? items[key] : null;\r\n    }\r\n    hasKey(key) {\r\n        let items = this.getItems();\r\n        return key in items;\r\n    }\r\n    removeItem(key) {\r\n        let items = this.getItems();\r\n        delete items[key];\r\n        this.setItems(items);\r\n    }\r\n    getItems() {\r\n        let items = window.localStorage.getItem(this.namespace);\r\n        return items !== null ? JSON.parse(localStorage.getItem(this.namespace)) : {};\r\n    }\r\n    setItems(items) {\r\n        window.localStorage.setItem(this.namespace, JSON.stringify(items));\r\n    }\r\n}\r\nexports.LocalStorage = LocalStorage;\r\n\n\n//# sourceURL=webpack:///./Scripts/services/localStorage.ts?");

/***/ }),

/***/ "./Scripts/services/pushNotification/nsPushNotification.ts":
/*!*****************************************************************!*\
  !*** ./Scripts/services/pushNotification/nsPushNotification.ts ***!
  \*****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\r\nvar __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {\r\n    return new (P || (P = Promise))(function (resolve, reject) {\r\n        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }\r\n        function rejected(value) { try { step(generator[\"throw\"](value)); } catch (e) { reject(e); } }\r\n        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }\r\n        step((generator = generator.apply(thisArg, _arguments || [])).next());\r\n    });\r\n};\r\nObject.defineProperty(exports, \"__esModule\", { value: true });\r\nconst pushNotificationBase_1 = __webpack_require__(/*! ./pushNotificationBase */ \"./Scripts/services/pushNotification/pushNotificationBase.ts\");\r\nclass NsPushNotification extends pushNotificationBase_1.PushNotificationBase {\r\n    init() {\r\n        return __awaiter(this, void 0, void 0, function* () {\r\n            return Promise.resolve(this);\r\n        });\r\n    }\r\n    isSupported() {\r\n        return false;\r\n    }\r\n    hasPermission() {\r\n        return false;\r\n    }\r\n    isDisabled() {\r\n        return true;\r\n    }\r\n    subscribe() {\r\n        return __awaiter(this, void 0, void 0, function* () {\r\n            throw new Error('Not supported');\r\n        });\r\n    }\r\n}\r\nexports.NsPushNotification = NsPushNotification;\r\n\n\n//# sourceURL=webpack:///./Scripts/services/pushNotification/nsPushNotification.ts?");

/***/ }),

/***/ "./Scripts/services/pushNotification/pushNotificationBase.ts":
/*!*******************************************************************!*\
  !*** ./Scripts/services/pushNotification/pushNotificationBase.ts ***!
  \*******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\r\nObject.defineProperty(exports, \"__esModule\", { value: true });\r\nclass PushNotificationBase {\r\n    constructor(_params) {\r\n        this._params = _params;\r\n    }\r\n}\r\nexports.PushNotificationBase = PushNotificationBase;\r\n\n\n//# sourceURL=webpack:///./Scripts/services/pushNotification/pushNotificationBase.ts?");

/***/ }),

/***/ "./Scripts/services/pushNotification/safariPushNotification.ts":
/*!*********************************************************************!*\
  !*** ./Scripts/services/pushNotification/safariPushNotification.ts ***!
  \*********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\r\nvar __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {\r\n    return new (P || (P = Promise))(function (resolve, reject) {\r\n        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }\r\n        function rejected(value) { try { step(generator[\"throw\"](value)); } catch (e) { reject(e); } }\r\n        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }\r\n        step((generator = generator.apply(thisArg, _arguments || [])).next());\r\n    });\r\n};\r\nObject.defineProperty(exports, \"__esModule\", { value: true });\r\nconst pushNotificationBase_1 = __webpack_require__(/*! ./pushNotificationBase */ \"./Scripts/services/pushNotification/pushNotificationBase.ts\");\r\nconst subscriptionInfo_1 = __webpack_require__(/*! ./subscriptionInfo */ \"./Scripts/services/pushNotification/subscriptionInfo.ts\");\r\nclass SafariPushNotification extends pushNotificationBase_1.PushNotificationBase {\r\n    init() {\r\n        return Promise.resolve(this);\r\n    }\r\n    isSupported() {\r\n        return 'safari' in window\r\n            && 'pushNotification' in window.safari\r\n            && 'Notification' in window;\r\n    }\r\n    hasPermission() {\r\n        if (!this.isSupported())\r\n            return false;\r\n        return this._getPermission() === 'granted';\r\n    }\r\n    isDisabled() {\r\n        if (!this.isSupported())\r\n            return true;\r\n        return this._getPermission() === 'denied';\r\n    }\r\n    subscribe() {\r\n        return __awaiter(this, void 0, void 0, function* () {\r\n            const subscription = yield this._subscribeInternal();\r\n            if (subscription.deviceToken == null) {\r\n                throw new Error(\"deviceToken is null. PermissionResponse: \" + JSON.stringify(subscription));\r\n            }\r\n            return new subscriptionInfo_1.SafariSubscriptionInfo(subscription.deviceToken);\r\n        });\r\n    }\r\n    _subscribeInternal() {\r\n        const promise = new Promise(resolve => {\r\n            window.safari.pushNotification.requestPermission(this._params.webServiceUrl, this._params.websitePushId, {}, resolve);\r\n        });\r\n        return promise;\r\n    }\r\n    _getPermission() {\r\n        return Notification.permission;\r\n    }\r\n}\r\nexports.SafariPushNotification = SafariPushNotification;\r\n\n\n//# sourceURL=webpack:///./Scripts/services/pushNotification/safariPushNotification.ts?");

/***/ }),

/***/ "./Scripts/services/pushNotification/subscriptionInfo.ts":
/*!***************************************************************!*\
  !*** ./Scripts/services/pushNotification/subscriptionInfo.ts ***!
  \***************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\r\nObject.defineProperty(exports, \"__esModule\", { value: true });\r\nclass W3cSubscriptionInfo {\r\n    constructor(data) {\r\n        this._p256dh = btoa(String.fromCharCode.apply(null, new Uint8Array(data.getKey('p256dh'))));\r\n        this._auth = btoa(String.fromCharCode.apply(null, new Uint8Array(data.getKey('auth'))));\r\n        this._endpoint = data.endpoint;\r\n    }\r\n    toJSON() {\r\n        return JSON.stringify({\r\n            endpoint: this._endpoint,\r\n            publicKey: this._p256dh,\r\n            auth: this._auth\r\n        });\r\n    }\r\n}\r\nexports.W3cSubscriptionInfo = W3cSubscriptionInfo;\r\nclass SafariSubscriptionInfo {\r\n    constructor(deviceToken) {\r\n        this.deviceToken = deviceToken;\r\n    }\r\n    toJSON() {\r\n        return JSON.stringify({ deviceToken: this.deviceToken });\r\n    }\r\n}\r\nexports.SafariSubscriptionInfo = SafariSubscriptionInfo;\r\n\n\n//# sourceURL=webpack:///./Scripts/services/pushNotification/subscriptionInfo.ts?");

/***/ }),

/***/ "./Scripts/services/pushNotification/w3cPushNotification.ts":
/*!******************************************************************!*\
  !*** ./Scripts/services/pushNotification/w3cPushNotification.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\r\nvar __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {\r\n    return new (P || (P = Promise))(function (resolve, reject) {\r\n        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }\r\n        function rejected(value) { try { step(generator[\"throw\"](value)); } catch (e) { reject(e); } }\r\n        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }\r\n        step((generator = generator.apply(thisArg, _arguments || [])).next());\r\n    });\r\n};\r\nObject.defineProperty(exports, \"__esModule\", { value: true });\r\nconst pushNotificationBase_1 = __webpack_require__(/*! ./pushNotificationBase */ \"./Scripts/services/pushNotification/pushNotificationBase.ts\");\r\nconst subscriptionInfo_1 = __webpack_require__(/*! ./subscriptionInfo */ \"./Scripts/services/pushNotification/subscriptionInfo.ts\");\r\nconst string_utils_1 = __webpack_require__(/*! ../../utils/string-utils */ \"./Scripts/utils/string-utils.ts\");\r\nclass W3cPushNotification extends pushNotificationBase_1.PushNotificationBase {\r\n    init() {\r\n        return __awaiter(this, void 0, void 0, function* () {\r\n            yield navigator.serviceWorker.register(this._params.serviceWorkerUrl);\r\n            return Promise.resolve(this);\r\n        });\r\n    }\r\n    isSupported() {\r\n        return 'serviceWorker' in navigator\r\n            && 'PushManager' in window\r\n            && 'Notification' in window;\r\n    }\r\n    hasPermission() {\r\n        if (!this.isSupported())\r\n            return false;\r\n        return this._getPermission() === 'granted';\r\n    }\r\n    isDisabled() {\r\n        if (!this.isSupported())\r\n            return true;\r\n        return this._getPermission() === 'denied';\r\n    }\r\n    subscribe() {\r\n        return __awaiter(this, void 0, void 0, function* () {\r\n            const swr = yield navigator.serviceWorker.getRegistration();\r\n            const subscription = yield swr.pushManager.subscribe({\r\n                userVisibleOnly: true,\r\n                applicationServerKey: string_utils_1.StringUtils.urlB64ToUint8Array(this._params.appPublicKey)\r\n            });\r\n            return new subscriptionInfo_1.W3cSubscriptionInfo(subscription);\r\n        });\r\n    }\r\n    _getPermission() {\r\n        return Notification.permission;\r\n    }\r\n}\r\nexports.W3cPushNotification = W3cPushNotification;\r\n\n\n//# sourceURL=webpack:///./Scripts/services/pushNotification/w3cPushNotification.ts?");

/***/ }),

/***/ "./Scripts/services/pushNotificationFactory.ts":
/*!*****************************************************!*\
  !*** ./Scripts/services/pushNotificationFactory.ts ***!
  \*****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\r\nObject.defineProperty(exports, \"__esModule\", { value: true });\r\nconst browser_1 = __webpack_require__(/*! ../utils/browser */ \"./Scripts/utils/browser.ts\");\r\nconst w3cPushNotification_1 = __webpack_require__(/*! ./pushNotification/w3cPushNotification */ \"./Scripts/services/pushNotification/w3cPushNotification.ts\");\r\nconst safariPushNotification_1 = __webpack_require__(/*! ./pushNotification/safariPushNotification */ \"./Scripts/services/pushNotification/safariPushNotification.ts\");\r\nconst nsPushNotification_1 = __webpack_require__(/*! ./pushNotification/nsPushNotification */ \"./Scripts/services/pushNotification/nsPushNotification.ts\");\r\nclass PushNotificationFactory {\r\n    static create(params) {\r\n        let instance = null;\r\n        switch (browser_1.Browser.getBrowserType()) {\r\n            case browser_1.BrowserType.Chrome:\r\n            case browser_1.BrowserType.Firefox:\r\n                instance = new w3cPushNotification_1.W3cPushNotification(params);\r\n                break;\r\n            case browser_1.BrowserType.Safari:\r\n                instance = new safariPushNotification_1.SafariPushNotification(params);\r\n                break;\r\n            default:\r\n                instance = new nsPushNotification_1.NsPushNotification(params);\r\n        }\r\n        return instance;\r\n    }\r\n}\r\nexports.PushNotificationFactory = PushNotificationFactory;\r\n\n\n//# sourceURL=webpack:///./Scripts/services/pushNotificationFactory.ts?");

/***/ }),

/***/ "./Scripts/utils/browser.ts":
/*!**********************************!*\
  !*** ./Scripts/utils/browser.ts ***!
  \**********************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\r\nObject.defineProperty(exports, \"__esModule\", { value: true });\r\nvar BrowserType;\r\n(function (BrowserType) {\r\n    BrowserType[BrowserType[\"Opera\"] = 0] = \"Opera\";\r\n    BrowserType[BrowserType[\"Firefox\"] = 1] = \"Firefox\";\r\n    BrowserType[BrowserType[\"Safari\"] = 2] = \"Safari\";\r\n    BrowserType[BrowserType[\"IE\"] = 3] = \"IE\";\r\n    BrowserType[BrowserType[\"Edge\"] = 4] = \"Edge\";\r\n    BrowserType[BrowserType[\"Chrome\"] = 5] = \"Chrome\";\r\n    BrowserType[BrowserType[\"Blink\"] = 6] = \"Blink\";\r\n})(BrowserType || (BrowserType = {}));\r\nexports.BrowserType = BrowserType;\r\nclass Browser {\r\n    static getBrowserType() {\r\n        if (this.isOpera)\r\n            return BrowserType.Opera;\r\n        if (this.isFirefox)\r\n            return BrowserType.Firefox;\r\n        if (this.isSafari)\r\n            return BrowserType.Safari;\r\n        if (this.isIE)\r\n            return BrowserType.IE;\r\n        if (this.isEdge)\r\n            return BrowserType.Edge;\r\n        if (this.isChrome)\r\n            return BrowserType.Chrome;\r\n        if (this.isBlink)\r\n            return BrowserType.Blink;\r\n        return null;\r\n    }\r\n}\r\nBrowser.isOpera = (!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;\r\nBrowser.isFirefox = typeof InstallTrigger !== 'undefined';\r\nBrowser.isSafari = /constructor/i.test(window.HTMLElement) || (function (p) { return p.toString() === \"[object SafariRemoteNotification]\"; })(!window['safari'] || (typeof safari !== 'undefined' && safari.pushNotification));\r\nBrowser.isIE = false || !!document.documentMode;\r\nBrowser.isEdge = !Browser.isIE && !!window.StyleMedia;\r\nBrowser.isChrome = !!window.chrome && !!window.chrome.webstore;\r\nBrowser.isBlink = (Browser.isChrome || Browser.isOpera) && !!window.CSS;\r\nexports.Browser = Browser;\r\n\n\n//# sourceURL=webpack:///./Scripts/utils/browser.ts?");

/***/ }),

/***/ "./Scripts/utils/string-utils.ts":
/*!***************************************!*\
  !*** ./Scripts/utils/string-utils.ts ***!
  \***************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\r\nObject.defineProperty(exports, \"__esModule\", { value: true });\r\nclass StringUtils {\r\n    static urlB64ToUint8Array(base64String) {\r\n        let padding = '='.repeat((4 - base64String.length % 4) % 4);\r\n        var base64 = (base64String + padding)\r\n            .replace(/\\-/g, '+')\r\n            .replace(/_/g, '/');\r\n        var rawData = window.atob(base64);\r\n        var outputArray = new Uint8Array(rawData.length);\r\n        for (var i = 0; i < rawData.length; ++i) {\r\n            outputArray[i] = rawData.charCodeAt(i);\r\n        }\r\n        return outputArray;\r\n    }\r\n}\r\nexports.StringUtils = StringUtils;\r\n\n\n//# sourceURL=webpack:///./Scripts/utils/string-utils.ts?");

/***/ }),

/***/ "./Scripts/wps.ts":
/*!************************!*\
  !*** ./Scripts/wps.ts ***!
  \************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\r\nObject.defineProperty(exports, \"__esModule\", { value: true });\r\nconst wpsManager_1 = __webpack_require__(/*! ./wpsManager */ \"./Scripts/wpsManager.ts\");\r\nconst infoMessage = document.getElementById('infoMessage');\r\nconst subscribeBtn = document.getElementById('subscribeBtn');\r\ninfoMessage.innerText = 'Loading...';\r\nsubscribeBtn.innerText = 'Subscribe';\r\nsubscribeBtn.disabled = true;\r\nconst manager = new wpsManager_1.WpsManager(wpsOptions);\r\nmanager.init()\r\n    .then(m => {\r\n    if (!m.isSupported()) {\r\n        infoMessage.innerText = 'This browser does not support Push Notifications';\r\n        return;\r\n    }\r\n    if (m.isDisabled()) {\r\n        infoMessage.innerText = 'Push Notifications are disabled. Please enable the Notifications and reload the page';\r\n        return;\r\n    }\r\n    subscribeBtn.disabled = false;\r\n    if (m.isSubscribed()) {\r\n        onSubscribe();\r\n    }\r\n    else {\r\n        infoMessage.innerText = 'This browser supports Push Notifications!';\r\n    }\r\n    subscribeBtn.addEventListener(\"click\", subscribeBtnClick);\r\n});\r\nfunction subscribeBtnClick(e) {\r\n    if (!manager.isSubscribed()) {\r\n        manager.subscribe()\r\n            .then(x => {\r\n            if (x) {\r\n                onSubscribe();\r\n                return;\r\n            }\r\n            infoMessage.innerText = 'Something went wrong!';\r\n        })\r\n            .catch(error => { infoMessage.innerText = error.message; });\r\n    }\r\n    else {\r\n        manager.unsubscribe()\r\n            .then(x => {\r\n            if (x) {\r\n                onUnsubscribe();\r\n                return;\r\n            }\r\n            infoMessage.innerText = 'Something went wrong!';\r\n        });\r\n    }\r\n}\r\nfunction onSubscribe() {\r\n    infoMessage.innerText = 'Subscribed!';\r\n    subscribeBtn.innerText = 'Unsubscribe';\r\n}\r\nfunction onUnsubscribe() {\r\n    infoMessage.innerText = 'Unsubscribed!';\r\n    subscribeBtn.innerText = 'Subscribe';\r\n}\r\n\n\n//# sourceURL=webpack:///./Scripts/wps.ts?");

/***/ }),

/***/ "./Scripts/wpsManager.ts":
/*!*******************************!*\
  !*** ./Scripts/wpsManager.ts ***!
  \*******************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\r\nvar __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {\r\n    return new (P || (P = Promise))(function (resolve, reject) {\r\n        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }\r\n        function rejected(value) { try { step(generator[\"throw\"](value)); } catch (e) { reject(e); } }\r\n        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }\r\n        step((generator = generator.apply(thisArg, _arguments || [])).next());\r\n    });\r\n};\r\nObject.defineProperty(exports, \"__esModule\", { value: true });\r\nconst localStorage_1 = __webpack_require__(/*! ./services/localStorage */ \"./Scripts/services/localStorage.ts\");\r\nconst pushNotificationFactory_1 = __webpack_require__(/*! ./services/pushNotificationFactory */ \"./Scripts/services/pushNotificationFactory.ts\");\r\nconst apiClient_1 = __webpack_require__(/*! ./services/apiClient */ \"./Scripts/services/apiClient.ts\");\r\nclass WpsManager {\r\n    constructor(_options) {\r\n        this._options = _options;\r\n        this._skey_userId = 'uid';\r\n        this._storage = new localStorage_1.LocalStorage();\r\n        this._pushNotification = pushNotificationFactory_1.PushNotificationFactory.create(_options.pushNotificationParams);\r\n    }\r\n    init() {\r\n        return __awaiter(this, void 0, void 0, function* () {\r\n            if (this._pushNotification === null)\r\n                return Promise.resolve(this);\r\n            yield this._pushNotification.init();\r\n            return Promise.resolve(this);\r\n        });\r\n    }\r\n    isSupported() {\r\n        return this._pushNotification.isSupported();\r\n    }\r\n    isSubscribed() {\r\n        return this._storage.hasKey(this._skey_userId);\r\n    }\r\n    hasPermission() {\r\n        return this._pushNotification.hasPermission();\r\n    }\r\n    isDisabled() {\r\n        return this._pushNotification.isDisabled();\r\n    }\r\n    subscribe() {\r\n        return __awaiter(this, void 0, void 0, function* () {\r\n            const subscriptionInfo = yield this._pushNotification.subscribe();\r\n            try {\r\n                const apiClient = new apiClient_1.ApiClient(this._options.subscriptionsEndpoint);\r\n                const userId = yield apiClient.save(subscriptionInfo);\r\n                if (userId !== null) {\r\n                    this._storage.setItem(this._skey_userId, userId);\r\n                    return true;\r\n                }\r\n                return false;\r\n            }\r\n            catch (_a) {\r\n                throw new Error('NetworkError when attempting to connect to subscriptionsEndpoint.');\r\n            }\r\n        });\r\n    }\r\n    unsubscribe() {\r\n        return __awaiter(this, void 0, void 0, function* () {\r\n            if (!this._storage.hasKey(this._skey_userId))\r\n                return;\r\n            const userId = this._storage.getItem(this._skey_userId);\r\n            const apiClient = new apiClient_1.ApiClient(this._options.subscriptionsEndpoint);\r\n            const isSuccess = yield apiClient.delete(userId);\r\n            if (isSuccess) {\r\n                this._storage.removeItem(this._skey_userId);\r\n                return true;\r\n            }\r\n            return false;\r\n        });\r\n    }\r\n}\r\nexports.WpsManager = WpsManager;\r\n\n\n//# sourceURL=webpack:///./Scripts/wpsManager.ts?");

/***/ })

/******/ });