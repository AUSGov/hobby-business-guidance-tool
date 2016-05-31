/*! modernizr 3.3.1 (Custom Build) | MIT *
 * http://modernizr.com/download/?-cookies-touchevents-setclasses !*/
!function (e, n, o) { function t(e, n) { return typeof e === n } function s() { var e, n, o, s, a, i, r; for (var l in c) if (c.hasOwnProperty(l)) { if (e = [], n = c[l], n.name && (e.push(n.name.toLowerCase()), n.options && n.options.aliases && n.options.aliases.length)) for (o = 0; o < n.options.aliases.length; o++) e.push(n.options.aliases[o].toLowerCase()); for (s = t(n.fn, "function") ? n.fn() : n.fn, a = 0; a < e.length; a++) i = e[a], r = i.split("."), 1 === r.length ? Modernizr[r[0]] = s : (!Modernizr[r[0]] || Modernizr[r[0]] instanceof Boolean || (Modernizr[r[0]] = new Boolean(Modernizr[r[0]])), Modernizr[r[0]][r[1]] = s), f.push((s ? "" : "no-") + r.join("-")) } } function a(e) { var n = u.className, o = Modernizr._config.classPrefix || ""; if (p && (n = n.baseVal), Modernizr._config.enableJSClass) { var t = new RegExp("(^|\\s)" + o + "no-js(\\s|$)"); n = n.replace(t, "$1" + o + "js$2") } Modernizr._config.enableClasses && (n += " " + o + e.join(" " + o), p ? u.className.baseVal = n : u.className = n) } function i() { return "function" != typeof n.createElement ? n.createElement(arguments[0]) : p ? n.createElementNS.call(n, "http://www.w3.org/2000/svg", arguments[0]) : n.createElement.apply(n, arguments) } function r() { var e = n.body; return e || (e = i(p ? "svg" : "body"), e.fake = !0), e } function l(e, o, t, s) { var a, l, f, c, d = "modernizr", p = i("div"), h = r(); if (parseInt(t, 10)) for (; t--;) f = i("div"), f.id = s ? s[t] : d + (t + 1), p.appendChild(f); return a = i("style"), a.type = "text/css", a.id = "s" + d, (h.fake ? h : p).appendChild(a), h.appendChild(p), a.styleSheet ? a.styleSheet.cssText = e : a.appendChild(n.createTextNode(e)), p.id = d, h.fake && (h.style.background = "", h.style.overflow = "hidden", c = u.style.overflow, u.style.overflow = "hidden", u.appendChild(h)), l = o(p, e), h.fake ? (h.parentNode.removeChild(h), u.style.overflow = c, u.offsetHeight) : p.parentNode.removeChild(p), !!l } var f = [], c = [], d = { _version: "3.3.1", _config: { classPrefix: "", enableClasses: !0, enableJSClass: !0, usePrefixes: !0 }, _q: [], on: function (e, n) { var o = this; setTimeout(function () { n(o[e]) }, 0) }, addTest: function (e, n, o) { c.push({ name: e, fn: n, options: o }) }, addAsyncTest: function (e) { c.push({ name: null, fn: e }) } }, Modernizr = function () { }; Modernizr.prototype = d, Modernizr = new Modernizr, Modernizr.addTest("cookies", function () { try { n.cookie = "cookietest=1"; var e = -1 != n.cookie.indexOf("cookietest="); return n.cookie = "cookietest=1; expires=Thu, 01-Jan-1970 00:00:01 GMT", e } catch (o) { return !1 } }); var u = n.documentElement, p = "svg" === u.nodeName.toLowerCase(), h = d._config.usePrefixes ? " -webkit- -moz- -o- -ms- ".split(" ") : ["", ""]; d._prefixes = h; var m = d.testStyles = l; Modernizr.addTest("touchevents", function () { var o; if ("ontouchstart" in e || e.DocumentTouch && n instanceof DocumentTouch) o = !0; else { var t = ["@media (", h.join("touch-enabled),("), "heartz", ")", "{#modernizr{top:9px;position:absolute}}"].join(""); m(t, function (e) { o = 9 === e.offsetTop }) } return o }), s(), a(f), delete d.addTest, delete d.addAsyncTest; for (var v = 0; v < Modernizr._q.length; v++) Modernizr._q[v](); e.Modernizr = Modernizr }(window, document);