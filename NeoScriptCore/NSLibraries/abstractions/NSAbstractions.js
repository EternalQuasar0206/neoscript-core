function _neoScriptSafeInject(toPrint) {
	if(typeof toPrint == "string") {
		document.body.innerText += toPrint;
	}
	else {
		$qs.element.inject('body', toPrint);
	}
}

function _neoScriptInject(toPrint) {
	if(typeof toPrint == "string") {
		document.body.innerHTML += toPrint;
	}
	else {
		$qs.element.inject('body', toPrint);
	}
}