// QuasarStack.js 1.0.0 [07/2021] (c) 2021 Kayky Vitor Cruz
if (!window.document) {
	throw new Error("QuasarStack: A document is required to execute the core.");
}
var htmlDictionary = [
'a', 'abbr', 'address', 'area', 'article', 'aside', 'audio', 'b', 'base', 'bdi', 'bdo', 'blockquote', 'body', 'br', 'button', 'canvas', 'caption',
'cite', 'code', 'col', 'colgroup', 'data', 'datalist', 'dd', 'del', 'details', 'dfn', 'dialog', 'div', 'dl', 'dt', 'em', 'embed', 'fieldset', 'figcaption',
'figure', 'footer', 'form', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'head', 'header', 'hr', 'html', 'i', 'iframe', 'img', 'input', 'ins', 'kbd', 'label', 'legend',
'li', 'link', 'main', 'map', 'mark', 'meta', 'meter', 'nav', 'noscript', 'object', 'o', 'optgroup', 'option', 'output', 'p', 'param', 'picture', 'pre', 'progress',
'q', 'rp', 'rt', 'ruby', 's', 'samp', 'script', 'section', 'select', 'small', 'source', 'span', 'strong', 'style', 'sub', 'summary', 'sup', 'svg', 'table', 'tbody',
'td', 'template', 'textarea', 'tfoot', 'th', 'thead', 'time', 'title', 'tr', 'track', 'u', 'ul', 'var', 'video', 'wbr'
];

var lastAppInfo = {
	icon: "",
	title: ""
};

function consPrint(text, type = "text") {
	switch(type) {
		case "error":
		console.error(text);
		break;
		
		case "warn":
		console.warn(text);
		break;
		
		case "text":
		console.log(text);
		break;
	}
}

var $qs = {
	defaultState: {
	},
	dvars: {
		pageContent: ""
	},
	system: {
		environment: {
			enableExternalLoad: true,
			enableSaveState: true,
			enableVirtualization: true
		},
		eventsTimeout: 100,
		userScrolling: false,
		loadedPlugins: [],
		load: function(url, callback) {
			
			if ($qs.system.environment.enableExternalLoad) {
				let script = document.createElement("script")
				script.type = "text/javascript";
				script.defer = true; 
				if (script.readyState) {
					script.onreadystatechange = function(){
						if (script.readyState == "loaded" || script.readyState == "complete") {
						script.onreadystatechange = null;
						callback;
						}
					}	
				} 
				else {
					script.onload = function() { callback(); };
				}
				
				script.src = url;
				$qs.element.inject('head', script);
			}
			else {
				consPrint(externalNotEnabled, "error");
			}
		},
		usePlugin: function(name) {
			loadedPlugins.push(name);
		},
		info: {
			buildName: "QuasarStack.js",
			collaborators: ["Kayky Vitor Cruz"],
			version: [1,0,0]
		},
		errors: {
			externalNotEnabled: "QuasarStack: External Loading is not enabled in the current application",
			virtNotEnabled: "QuasarStack: Virtualization is not enabled in the current application",
			invalidHtml: "The parameters entered do not refer to a valid html or it has been discontinued."
		}
	},
	date: {
		now: function() {
			return new Date().toLocaleString();
		}
	},
	data: {
		vardump: function (i) {
			let type = typeof i;
			let length = i.length;
			
			if (type == 'array') {
				let l = 0;
				i.forEach(function(x){
					l++;
				});
				length = l;
			}
			
			if (type == 'object') {
				let l = 0;
				Object.keys(i).forEach(function(x){
					l++;
				});
				length = l;
			}
			
			consPrint("\nType: " + typeof i + "\nLength: " + i.length);
			return {type, length};
		},
		check: {
			element: function(i) {
				try {
					return i instanceof HTMLElement || i instanceof Element;
				}
				catch(e){
					return (typeof obj=== "object") &&
					(i.nodeType===1) && (typeof i.style === "object") &&
					(typeof i.ownerDocument === "object");
				}
			},
			email: function(i) {
				const em = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
				return em.test(i);
			},
			phone: function(i) {
				const em = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;
				return em.test(i);
			}		
		},
		num: {
			list: ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"],
			remove: function(content) {
						let finalContent = "";
	
						try {
						let parsedContent = content.toString();
							for (let i = 0; i < parsedContent.length; i++) {
								if (!$qs.data.num.list.includes(parsedContent.charAt(i))) {
									finalContent += parsedContent.charAt(i);
								}
							}
						} catch (e) {
						consPrint("QuasarStack: " + e, "error");
					}
	
					return finalContent;
			},
			filter: function(content) {
						let finalContent = "";
	
						try {
						let parsedContent = content.toString();
							for (let i = 0; i < parsedContent.length; i++) {
								if ($qs.data.num.list.includes(parsedContent.charAt(i))) {
									finalContent += parsedContent.charAt(i);
								}
							}
						} catch (e) {
						consPrint("QuasarStack: " + e, "error");
					}
	
					return finalContent;
				}
			}
	}, 
	app: {
		appTitle: "",
		appDescription: "",
		appVersion: [1, 0, 0],
		appAuthor: "",
		appIcon: "",
		scrollTo: async function(target, plus = 0, minus = 0) {
			window.scroll(0, $qs.element.find(target).offsetTop);
		}
	},
	element: {
		find: function(target) {
			return document.querySelector(target);
		},
		
		new: function(objType, propObj = {}) {
			let finalElement;
	
			if (htmlDictionary.includes(objType)) {
				finalElement = document.createElement(objType);
				Object.keys(propObj).forEach(function (i) {
					if (i == 'content') {
						finalElement.innerHTML = propObj[i];
					}
					else if (i.startsWith('$')) {
						finalElement.innerHTML += propObj[i];
					}
					else {
						finalElement.setAttribute(i, propObj[i]);
					}
				});
			}
			else {
			consPrint("QuasarStack: " + $qs.system.errors.invalidHtml, "error");
			}
	
			return finalElement;
		},
		
		instantiate: function(obj, props = {}) {
			let finalElement = obj;
			
			Object.keys(props).forEach(function (i) {
				finalElement.innerHTML = finalElement.innerHTML.replaceAll(i, props[i]);
			});
			
			return finalElement;
		},
		
		destroy: async function(obj, timeout = 0) {
			await new Promise(t => setTimeout(t, timeout * 1000));
			$qs.element.find(obj).parentElement.removeChild($qs.element.find(obj));
		},
		
		pick: function(obj) {
			$qs.element.destroy(obj);
			return $qs.element.find(obj);
		},
		
		clear: function(obj) {
			$qs.element.find(obj).innerHTML = "";
		},
		
		hide: function(obj) {
			$qs.element.find(obj).style.display = 'none';
		},
		
		show: function(obj, newDisplayType = '') {
			$qs.element.find(obj).style.display = newDisplayType;
		},
		
		update: function(obj, argument) {
			if (typeof argument == "string") {
				if ($qs.system.enableVirtualization) {
					let objContent = $qs.element.find(obj).innerHTML;
					if (!$qs_compare(objContent, argument)) {
						$qs.element.find(obj).innerHTML = argument;
					}
				}
				else {
					$qs.element.find(obj).innerHTML = argument;
				}
			}
			else {
				$qs.element.find(obj).innerHTML = "";
				$qs.element.find(obj).appendChild(argument).cloneNode(true);
			}
		},
		
		inject: function(target, obj) {
			if (typeof target == "string") {
				if(!Array.isArray(obj)) {
					$qs.element.find(target).appendChild(obj).cloneNode(true);
				}
				else {
					obj.forEach(function(i) {
						$qs.element.inject(target, i);
					});
				}
			}
			else {
				if(!Array.isArray(obj)) {
					target.appendChild(obj).cloneNode(true);
				}
				else {
					obj.forEach(function(i) {
						$qs.element.inject(target, i);
					});
				}
			}
		},
		
		setParent: function(target, origin) {
			$qs.element.find(target).appendChild(origin);
		},
		
		setStyle: function(obj, style) {
			document.querySelector(obj).style.cssText = style;
		},
		
		addStyle: function(obj, style) {
			document.querySelector(obj).style.cssText += style;
		},
		
		foreColor: function(obj, color) {
			document.querySelector(obj).style.color = color;
		},
		
		backColor: function(obj, color) {
			document.querySelector(obj).style.backgroundColor = color;
		},
		
		resize: function(obj, args) {
			var divisoryq = 0;
			var args_arr;
	
			for (var i = 0; i < args.length; i++) {
				if(args.charAt(i) == 'x' || args.charAt(i) == 'X') {
					divisoryq++;
				}
			}
	
			if (divisoryq == 1) {
				args = args.replaceAll('x', 'X');
				args_arr = args.split('X');
				document.querySelector(obj).style.width = numFilter(args_arr[0]) + "px";
				document.querySelector(obj).style.height = numFilter(args_arr[1]) + "px";
			}
		},
		
		scale: function(obj, args) {
			$qs.element.addStyle(obj, 'transform: scale(' + args + ');');
		},
		
		rotate: function(obj, args) {
			let argument = args.toString().toLowerCase();
			if (argument.startsWith("x")) {
				$qs.element.addStyle(obj, 'transform: rotatex(' + $qs.data.num.filter(argument) + 'deg);');
			}
			else if (argument.startsWith("y")) {
				$qs.element.addStyle(obj, 'transform: rotatey(' + $qs.data.num.filter(argument) + 'deg);');
			}
			else {
				$qs.element.addStyle(obj, 'transform: rotate(' + $qs.data.num.filter(argument) + 'deg);');
			}
		},
		
		rotate3d: function(obj, x, y, z, a) {
			$qs.element.addStyle(obj, 'transform: rotate3d(' + x + ', ' + y + ', ' + z + ', ' + a + 'deg);');
		},
		
		animationTime: function(obj, args) {
			$qs.element.addStyle(obj, 'transition-duration: + ' + args +  's;');
		}
			
	},
	elementAll: {
		find: function(obj) {
			return document.querySelectorAll(obj);
		},
		
		destroy: async function(obj, timeout = 0) {
			await new Promise(t => setTimeout(t, timeout * 1000));
			$qs.elementAll.find(obj).forEach(function(i) {
				i.parentElement.removeChild(i);
			});
		},
		
		clear: function(obj) {
			$qs.elementAll.find(obj).forEach(function(i) {
				i.innerHTML = "";
			});
		},
		
		hide: function(obj) {
			$qs.elementAll.find(obj).forEach(function(i) {
				$qs.element.hide(i);
			});
		},
		
		show: function(obj, newDisplayType = '') {
			$qs.elementAll.find(obj).forEach(function(i) {
				$qs.element.show(i, newDisplayType);
			});
		},
		
		pick: function(obj) {
			let finalElements = [];
			$qs.elementAll.find(obj).forEach(function(i) {
				finalElements.push(i);
				i.parentElement.removeChild(i);
			});
			
			return finalELements;
		},
		
		update: function(obj, argument) {
			$qs.elementAll.find(obj).forEach(function(i) {
				$qs.element.update(i, argument);
			});
		},
		
		inject: function(obj, argument) {
			$qs.elementAll.find(obj).forEach(function(i) {
				$qs.element.inject(i, argument);
			});
		},
		
		setParent: function(target, origin) {
			$qs.elementAll.find(target).forEach(function(i) {
				$qs.element.setParent(i, origin);
			});
		},
		
		setStyle: function(object, style) {
			$qs.elementAll.find(object).forEach(function(i) {
				$qs.element.setStyle(i, style);
			});
		},
		
		addStyle: function(object, style) {
			$qs.elementAll.find(object).forEach(function(i) {
				$qs.element.addStyle(i, style);
			});
		},
		
		foreColor: function(object, color) {
			$qs.elementAll.find(object).forEach(function(i) {
				$qs.element.foreColor(i, color);
			});
		},
		
		backColor: function(object, color) {
			$qs.elementAll.find(object).forEach(function(i) {
				$qs.element.backColor(i, color);
			});
		},
		
		resize: function(object, args) {
			$qs.elementAll.find(object).forEach(function(i) {
				$qs.element.resize(i, args);
			});
		},
		
		scale: function(object, args) {
			$qs.elementAll.find(object).forEach(function(i) {
				$qs.element.scale(i, args);
			});
		},
		
		rotate: function(object, args) {
			$qs.elementAll.find(object).forEach(function(i) {
				$qs.element.rotate(i, args);
			});
		},
		
		rotate3d: function(object, x, y, z, a) {
			$qs.elementAll.find(object).forEach(function(i) {
				$qs.element.rotate3d(i, x, y, z, a);
			});
		},
		
		animationTime: function(object, args) {
			$qs.elementAll.find(object).forEach(function(i) {
				$qs.element.animationTime(i, args);
			});
		}
	},
	action: {
		version: function() {
			return $qs.system.info.version.toString().replaceAll(',', '.');
		},
		reset: function(fullReset = false) {
			if(fullReset) window.location.href = window.location.href;
			else location.reload();
		},
		appInfo: function() {
			return [$qs.app.appTitle, $qs.app.appDescription, $qs.app.appVersion.toString().replaceAll(',', '.')];
		},
		disableZoom: function(disable = true) {
			if (disable) {
				document.body.addEventListener("wheel", e=>{
					if(e.ctrlKey) e.preventDefault();
				}, { passive: false });
			}
			else {
				document.body.removeEventListener("wheel", e=>{
					if(e.ctrlKey) e.preventDefault();
				}, { passive: false });
			}
		},
		browserInfo: function(nav = window.navigator) {
			var data = {};
			var ua = data.uaString = nav.userAgent;
			var browserMatch = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*([\d\.]+)/i) || [];
			if (browserMatch[1]) { browserMatch[1] = browserMatch[1].toLowerCase(); }
			var operaMatch = browserMatch[1] === 'chrome';
			if (operaMatch) { operaMatch = ua.match(/\bOPR\/([\d\.]+)/); }

			if (/trident/i.test(browserMatch[1])) {
				var msieMatch = /\brv[ :]+([\d\.]+)/g.exec(ua) || [];
				data.name = 'msie';
				data.version = msieMatch[1];
			}
			else if (operaMatch) {
				data.name = 'opera';
				data.version = operaMatch[1];
			}
			else if (browserMatch[1] === 'safari') {
				var safariVersionMatch = ua.match(/version\/([\d\.]+)/i);
				data.name = 'safari';
				data.version = safariVersionMatch[1];
			}
			else {
				data.name = browserMatch[1];
				data.version = browserMatch[2];
			}

			var versionParts = [];
			if (data.version) {
				var versionPartsMatch = data.version.match(/(\d+)/g) || [];
				for (var i=0; i < versionPartsMatch.length; i++) {
					versionParts.push(versionPartsMatch[i]);
				}
				if (versionParts.length > 0) { data.majorVersion = versionParts[0]; }
			}
			data.name = data.name || '(Unknown browser name)';
			data.version = {
				full: data.version || '(Unknown full browser version)',
				parts: versionParts,
				major: versionParts.length > 0 ? versionParts[0] : '(Unknown major browser version)'
			};

			return data;
		},
		viewportTrigger: async function(timeout = 100, fullReset = false) {
			await new Promise(r => setTimeout(r, timeout));
  
			if (window.innerWidth != startupWidth) {
				$qs.action.reset(fullReset);
			}
			
			$qs.action.viewportTrigger(timeout);
		},
		update: async function(todo, interval) {
			eval(todo);
			await new Promise(r => setTimeout(r, interval));
			$qs.action.update(todo, interval);
		}
	},
	chain: {
		update: async function(target, stateName, timeout) {
			if($qs.system.environment.enableVirtualization) {
				let finalContent = $qs.defaultState[stateName];
				let index = 0;
			
				for (const item in $qs.dvars) {
					finalContent = finalContent.replaceAll('{' + item + '}', $qs.dvars[item]);
				}
			
				if (finalContent != target.innerHTML) {
					target.innerHTML = finalContent;
				}
			
				await new Promise(r => setTimeout(r, timeout));
				$qs.chain.update(target, stateName, timeout);
			}
			else {
				consPrint($qs.system.errors.virtNotEnabled, "error");
			}
		},
		
		safeUpdate: async function(target, stateName, timeout) {
			if($qs.system.environment.enableVirtualization) {
				let finalContent = $qs.defaultState[stateName];
				let index = 0;
			
				for (const item in $qs.dvars) {
					finalContent = finalContent.replaceAll('{' + item + '}', $qs.dvars[item]);
				}
			
				if (finalContent != target.innerText) {
					target.innerText = finalContent;
				}
			
				await new Promise(r => setTimeout(r, timeout));
				$qs.chain.safeUpdate(target, stateName, timeout);
			}
			else {
				consPrint($qs.system.errors.virtNotEnabled, "error");
			}
		}
	}
};

function $set(varName, defaulContent = "") { //=> Set Dynamic Variable
	$qs.dvars[varName] = defaulContent;
}

function $saveState(stateName, object) {
	if ($qs.system.environment.enableSaveState) {
		$qs.defaultState[stateName] = $qs.element.find(object).innerHTML;
	}
	else {
		consPrint("QuasarStack: Save State is not enabled in the current application", "error");
	}
}

//QuasarStack Events
if ($qs.system.environment.enableVirtualization) { $qs.dvars.pageContent = document.body.innerHTML; }
document.addEventListener("wheel", function() { $qs.system.userScrolling = true; });

lastAppIcon = $qs.app.appIcon;

//Main events time loop
consPrint("QuasarStack: Core initialized. Version: " + $qs.action.version());
$qs.action.update(`
	$qs.system.userScrolling = false;
	
	if ($qs.app.appIcon != lastAppInfo.icon) {
		let link = document.querySelector("link[rel~='icon']");
			if (!link) {
				link = document.createElement('link');
				link.rel = 'icon';
				document.getElementsByTagName('head')[0].appendChild(link);
			}
		if($qs.app.appIcon.endsWith('.png')) {
			link.type = "image/png";
		} else {
			link.type = "image/x-icon";
		}
		link.href = $qs.app.appIcon;
		lastAppInfo.icon = $qs.app.appIcon;
	}
	
	if ($qs.app.appTitle != lastAppInfo.title) {
		document.title = $qs.app.appTitle;
		lastAppInfo.title = $qs.app.appTitle;
	}
`, $qs.system.eventsTimeout);

function $qs_compare(oldValue, newValue) {
	if (oldValue === newValue) {
		return true;
	}
	return false;
}
