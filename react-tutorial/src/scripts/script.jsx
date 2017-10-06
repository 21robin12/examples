// ReSharper disable Es6Feature
class BlogPost extends React.Component {
    render() {
        return (
            <div className="blog-post">
                {this.props.description}
            </div>
        );
    }
}

class BlogFeed extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            posts: [
                { text: "This is my first blog post ever!", id: 1 },
                { text: "Here's another blog post here", id: 2 },
                { text: "Last blog post for now", id: 3 }
            ]
        };

        this.addPost = this.addPost.bind(this);
    }

    addPost() {
        var posts = this.state.posts;
        var lastId = posts[posts.length - 1].id;
        posts.push({text: "Another blog post added here", id: lastId + 1});
        this.setState({ posts: posts });
    }

    render() {
        // React tutorial says list items need a unique "key": https://facebook.github.io/react/docs/lists-and-keys.html
        // however, everything seems to be working for now without a key. this says that we should definitely add unique keys, 
        // or weird things will start happening: https://coderwall.com/p/jdybeq/the-importance-of-component-keys-in-react-js
        const blogPosts = this.state.posts.map((post) => 
            <BlogPost description={post.text} key={post.id} />
        );

        // must return a single root-level element; hence the div
        return (
            <div>
                <h4>{this.props.heading}</h4>
                {blogPosts}
                <button onClick={this.addPost}>Add post</button>
            </div>
        );
    }
}

class Clock extends React.Component {
    constructor(props) {
        super(props);
        // only place we can assign state directly is in the constructor
        this.state = { date: new Date(), toggleButtonText: "Pause" };

        // apparently this is necessary to make "this" work in pause method
        // doesn't need to be a part of "state" since it isn't rendered in the view and so doesn't need to be tracked by React
        this.togglePaused = this.togglePaused.bind(this);
    }

    start() {
        // must use "setstate" anywhere outside of constructor
        this.setState({ toggleButtonText: "Pause" });
        this.timerID = setInterval(
            () => this.tick(),
            1000
        );

        this.isRunning = true;
    }

    stop() {
        this.setState({ toggleButtonText: "Resume" });
        clearInterval(this.timerID);
        this.timerID = null;
        this.isRunning = false;
    }

    componentDidMount() {
        this.start();
    }

    componentWillUnmount() {
        console.log("Clock.componentWillUnmount called");
        this.stop();
    }

    tick() {
        this.setState({
            date: new Date()
        });
    }

    togglePaused() {
        if (this.isRunning) {
            this.stop();
        } else {
            this.start();
        }
    }

    render() {
        var isMorning = new Date().getHours() < 12;
        
        // variables can store elements for conditional rendering
        const welcome = isMorning ? <h1>Morning, world!</h1> : <h1>Hello, world!</h1>;

        return (
            <div>
                {welcome}
                <h2>It is {this.state.date.toLocaleTimeString()}.</h2>
                <button onClick={this.togglePaused }>{this.state.toggleButtonText}</button>
            </div>
        );
    }
}

class Unmounter extends React.Component {
    render() {
        return <button onClick={this.unMount}>Unmount React App</button>;
    }

    unMount() {
        console.log("unmounting...");
        ReactDOM.unmountComponentAtNode(document.getElementById("react-app"));
    }
}

class App extends React.Component {
    render() {
        return (
            <div>
                <Clock />
                <BlogFeed heading="Feed"/>
                <Unmounter />
            </div>
        );
    }
}
       
const app = <App />;
        
ReactDOM.render(app, document.getElementById("react-app"));