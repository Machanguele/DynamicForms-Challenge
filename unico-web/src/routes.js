// @flow
import React from 'react';
import {BrowserRouter, Switch, Route} from 'react-router-dom';
import {Questions} from "./pages/questions/Questions";
import {Answer} from "./pages/answers/Answer";
import {Inquiries} from "./pages/Inquiries/Inquiries";

export const Routes = () => {
    return (
        <BrowserRouter>
            <Switch>
                <Route path="/"  exact component={Inquiries}/>
                <Route path="/questions" component={Questions}/>
                <Route path="/answers" component={Answer}/>
            </Switch>
        </BrowserRouter>
    );
};