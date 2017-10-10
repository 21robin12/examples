var mvvm = new ModelViewViewModel();

function ObjectItem(settings) {
    var self = this;

    self.title = settings.title;
    self.number = mvvm.observable(settings.number);

    self.increaseNumber = function () {
        self.number(self.number() + 1);
        console.log(self.number());
    };
}

function DemoViewModel() {
    var self = this;
    
    self.loadingText = mvvm.observable("loading, please wait");
    self.dots = mvvm.observable("...");

    self.items = mvvm.observable(["first", "second", "third"]);

    self.objectItems = mvvm.observable([
        new ObjectItem({ title: "my first blog post", number: 1 }),
        new ObjectItem({ title: "another blog post", number: 2 })
    ]);

    self.updateLoadingText = function () {
        self.loadingText("updating loading text here");
    };

    self.updateItems = function () {
        var items = self.items();
        items.push("another item!!!");
        self.items(items);
    };

    self.updateObjectItems = function () {
        var objectItems = self.objectItems();
        var last = objectItems[objectItems.length - 1];
        objectItems.push(new ObjectItem({ title: "new blog post here", number: last.number() + 1 }));
        self.objectItems(objectItems);
    };

    self.displayTitle = mvvm.observable(true);
    self.showTitle = () => self.displayTitle(true);
    self.hideTitle = () => self.displayTitle(false);
}

var demoViewModel = new DemoViewModel();
document.addEventListener("DOMContentLoaded", function () {
    mvvm.bind(document.getElementById("mount-point"), demoViewModel);
});