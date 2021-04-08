import React from 'react';
import { MdQuestionAnswer } from "react-icons/md";
import {NavItem, Navbar, Button, Nav, NavDropdown, FormGroup, Form, FormControl } from 'react-bootstrap';
import '../styles/Header.css';
import {Link} from 'react-router-dom';

function Header() {
    return (
        <div className={"navbarContainer"}>
            <Navbar bg="light" expand="lg"   className="navbar" fixed="top">
                <MdQuestionAnswer color={'#58D68D'}/>
                <Navbar.Brand href="/" className="unicoForms" >#unicoForms</Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="mr-auto">
                        <Nav.Link href={"/"}>Home</Nav.Link>
                        <Nav.Link href="/questions">Questions</Nav.Link>
                        <Nav.Link href="/">Admin Painel</Nav.Link>
                    </Nav>
                </Navbar.Collapse>
            </Navbar>

        </div>
    );
}

export default Header;
