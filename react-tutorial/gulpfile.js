var gulp = require("gulp");
var babel = require("gulp-babel");
var concat = require('gulp-concat');

function swallowError(error) {
    console.log(error.toString());
    this.emit("end");
}

gulp.task("scripts", function () {
    return gulp.src("src/scripts/**/*.jsx")
        .pipe(concat("global.js"))
        .on("error", swallowError)
        .pipe(babel())
        .pipe(gulp.dest("dist/scripts"));
});

gulp.task("watch", function () {
    gulp.watch("src/scripts/**/*.jsx", ["scripts"]);
});

gulp.task("default", function () {
    // can pass many tasks here as an array
    return gulp.start(["scripts", "watch"]);
});