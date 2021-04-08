import './App.css';
import Header from "./components/Header";
import React from "react";
import {Routes} from "./routes";

function App() {
  return (
    <div className="App">
        <Header />
        <Routes />
    </div>
  );
}

export default App;
