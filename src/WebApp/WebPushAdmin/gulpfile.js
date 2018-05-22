"use strict";

var gulp = require("gulp"),
	concat = require("gulp-concat"),
	uglify = require("gulp-uglify"),
	merge = require("merge-stream"),
	gulpif = require('gulp-if'),
	sourcemaps = require('gulp-sourcemaps'),
	sass = require("gulp-sass"),
	bundles = require("./gulpbundle.json");

gulp.task("publish", ["lib", "publish:js", "publish:css"]);

gulp.task("lib", function () {
	return bundles.lib.map(function (lib) {
		return gulp.src(bundles.path.library + lib.inputFileName)
			.pipe(gulp.dest(bundles.path.outputLib));
	});
});

gulp.task("dev:js", function () {
	var tasks = runJs(false);
	return merge(tasks);
});

gulp.task("dev:sass", function () {
	return runSass(false, true);
});

gulp.task("publish:js", function () {
	var tasks = runJs(true);
	return merge(tasks);
});

gulp.task("publish:css", function () {
	var tasks = runSass(true, false);
	return merge(tasks);
});

gulp.task("watch", function () {
	gulp.watch(bundles.path.inputJS + "**/*.js", ["dev:js"]);
	gulp.watch(bundles.path.inputSASS + "**/*.scss", ["dev:sass"]);
});

function runJs(compress) {
	return bundles.js.map(function (bundle) {
		var files = bundle.inputFiles.map(function (name) {
			return bundles.path.inputJS + name;
		});
		return gulp.src(files, { base: "." })
			.pipe(concat(bundle.outputFileName))
			.pipe(gulpif(!!compress, uglify()))
			.pipe(gulp.dest("."));
	});
}

function runSass(compress, sourcemap) {
	return gulp.src(bundles.path.inputSASS + "**")
		.pipe(gulpif(!!sourcemap, sourcemaps.init()))
		.pipe(sass({ outputStyle: !!compress ? 'compressed' : null }))
		.pipe(gulpif(!!sourcemap, sourcemaps.write()))
		.pipe(gulp.dest(bundles.path.outputCSS));
}