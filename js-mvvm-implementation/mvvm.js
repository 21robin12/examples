function Subscriber(element, viewModel, bindingExpression, modelInitScript, mvvm) {
    var self = this;

    self.element = element;
    self.viewModel = viewModel;
    self.bindingExpression = bindingExpression;
    self.modelInitScript = modelInitScript;
    self.mvvm = mvvm;
}

function ValueSubscriber(element, viewModel, bindingExpression, modelInitScript, mvvm) {
    Subscriber.apply(this, arguments);
    var self = this;
    
    self.update = function () {
        eval(modelInitScript);
        var result = eval(bindingExpression);
        self.element.innerHTML = result;
    };

    self.update();
}

function ClickSubscriber(element, viewModel, bindingExpression, modelInitScript, mvvm) {
    Subscriber.apply(this, arguments);
    var self = this;

    self.update = function () { };

    self.element.addEventListener("click", function () {
        eval(modelInitScript);
        eval(bindingExpression);
    });
}

function ForeachSubscriber(element, viewModel, bindingExpression, modelInitScript, mvvm) {
    Subscriber.apply(this, arguments);
    var self = this;

    self.children = [];
    for (var i = 0; i < element.children.length; i++) {
        var cloned = element.children[i].cloneNode(true);
        self.children.push(element.children[i].cloneNode(true));
    }

    self.update = function () {
        eval(modelInitScript);
        var array = eval(bindingExpression);

        element.innerHTML = "";
        for (var i = 0; i < array.length; i++) {
            var arrayItem = array[i];
            for (var j = 0; j < self.children.length; j++) {
                var cloned = self.children[j].cloneNode(true);
                element.appendChild(cloned);
                mvvm.bind(cloned, arrayItem, "$data");
            }
        }
    };

    self.update();
}

function IfSubscriber(element, viewModel, bindingExpression, modelInitScript, mvvm) {
    Subscriber.apply(this, arguments);
    var self = this;

    self.defaultDisplay = element.style.display;

    self.update = function () {
        eval(modelInitScript);
        var result = eval(bindingExpression);
        if (result === true) {
            self.element.style.display = self.defaultDisplay;
        } else {
            self.element.style.display = "none";
        }
    };

    self.update();
}

function Observable(initialValue) {
    var self = this;

    self.value = initialValue;

    var accessor = function (value) {
        if (arguments.length === 0) { // get
            return self.value;
        } else { // set
            self.value = value;
            for (var i = 0; i < accessor.subscribers.length; i++) {
                accessor.subscribers[i].update();
            }
        }
    };

    accessor.isObservable = true;
    accessor.subscribers = [];

    return accessor;
}

function ModelViewViewModel() {
    var self = this;

    function extractVariableNames(expression, modelName) {
        var variableNames = [];

        var modelPrefix = modelName + ".";
        var getIndex = () => expression.indexOf(modelPrefix);
        var index = getIndex();
        while (index > -1) {
            var startingWithVariable = expression.substring(index + modelPrefix.length);
            var variableLength = startingWithVariable.search(/[^_$A-Za-z0-9]/);
            var variable = startingWithVariable.substring(0, variableLength);
            variableNames.push(variable);

            expression = startingWithVariable.substring(variableLength);
            index = getIndex();
        }

        return variableNames;
    }

    function getSubscriber(bindingType, bindingElement, viewModel, bindingExpression, modelName) {
        var modelInitScript = "var " + modelName + " = viewModel;";
        switch (bindingType) {
            case "value":
                return new ValueSubscriber(bindingElement, viewModel, bindingExpression, modelInitScript, self);
            case "click":
                return new ClickSubscriber(bindingElement, viewModel, bindingExpression, modelInitScript, self);
            case "foreach":
                return new ForeachSubscriber(bindingElement, viewModel, bindingExpression, modelInitScript, self);
            case "if":
                return new IfSubscriber(bindingElement, viewModel, bindingExpression, modelInitScript, self);
            default:
                throw new Error("Binding type '" + bindingType + "' not available");
        }
    }

    function bindElement(bindingType, element, viewModel, bindingExpression, modelName) {
        var variableNames = extractVariableNames(bindingExpression, modelName);
        var subscriber = getSubscriber(bindingType, element, viewModel, bindingExpression, modelName);

        for (var j = 0; j < variableNames.length; j++) {
            var variable = viewModel[variableNames[j]];
            if (variable && variable.isObservable) {
                variable.subscribers.push(subscriber);
            }
        }
    }

    function bindElements(elements, viewModel, modelName) {
        for (var i = 0; i < elements.length; i++) {
            var element = elements[i];
            var bindChildren = true;
            if (element.hasAttribute("data-bind")) {
                var dataBind = element.attributes["data-bind"].nodeValue;
                var split = dataBind.split(":");
                var bindingType = split[0].trim();
                var bindingExpression = split[1].trim();
                bindElement(bindingType, element, viewModel, bindingExpression, modelName);

                bindChildren = bindingType !== "foreach";
            }

            if (bindChildren) {
                bindElements(element.children, viewModel, modelName);
            }
        }
    }

    self.observable = function (initialValue) {
        var observable = new Observable();
        observable(initialValue);
        return observable;
    };

    self.bind = function (element, viewModel, modelName) {
        bindElements([element], viewModel, modelName ? modelName : "$model");
    };
}