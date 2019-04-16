'use strict';

// Provide a title to the process in `ps`.
// Due to an obscure Mac bug, do not start this title with any symbol.
process.title = 'setlinks';

const fs = require('fs-extra');
const path = require('path');

let unlink = process.argv.length > 2 ? process.argv[2] == "unlink" : false;



let corePath = path.resolve(__dirname, "../../EasyQuery.JS/packs/core");
let brokerPath = path.resolve(__dirname, "../../EasyQuery.JS/packs/broker-eqs");
let uiPath = path.resolve(__dirname, "../../EasyQuery.JS/packs/ui");
let uijqPath = path.resolve(__dirname, "../../EasyQuery.JS/packs/ui-jquery");


if (!unlink) {
	fs.removeSync("../EqAspNetCoreDemo/node_modules/@easyquery"); 
}

// -------------- demo projects --------------

link(uiPath, path.resolve(__dirname, "../EqAspNetCoreDemo/node_modules/@easyquery/ui"));
link(uijqPath, path.resolve(__dirname, "../EqAspNetCoreDemo/node_modules/@easyquery/ui-jquery"));



function link(to, from) {
	to = trim(to.toLowerCase());
	from = trim(from.toLowerCase());

	if (to === from) throw new Error(`Symlink path is the same as the target path (${to})`);

	if (unlink) {
		try {
			fs.unlinkSync(from);
		} catch (err) {} 
	}
	else {
		let eqDir = RemoveLastDirectoryPartOf(from);
		let nmDir = RemoveLastDirectoryPartOf(eqDir);

		if (!fs.existsSync(nmDir)) {
			fs.mkdirSync(nmDir, {recursive: true});
		}
		if (!fs.existsSync(eqDir)) {
			fs.mkdirSync(eqDir, {recursive: true});
		}
		
		try {
			fs.symlinkSync(to, from, 'junction');
		} catch (err) {
			if (err.code !== 'EEXIST') throw err;
		} 
	  
		let linkString;
		try {
			linkString = fs.readlinkSync(from);
		} catch (err) {
		// from is not a link
			throw new Error(`Source path (${from}) is busy by the folder`);
		}

		linkString = trim(linkString.toLowerCase());
		if (to !== linkString) {
			fs.unlinkSync(from);
			return link(to, from);  
		}
	}
}

function trim(path)     
{     
	let res = path.replace(/\\$/, "");
	return res;
} 

function RemoveLastDirectoryPartOf(dir)
{
    var the_arr = dir.split(path.sep);
    the_arr.pop();
    return( the_arr.join(path.sep) );
}