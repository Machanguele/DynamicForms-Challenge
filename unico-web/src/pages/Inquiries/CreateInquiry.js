import {InquiryCard} from "../../components/InquiryCard";
import React, {useEffect, useState} from "react";
import '../../styles/inquiry.css'
import {API_URL, apiCall} from "../../helpers/api";
import {MdAddToPhotos} from 'react-icons/md';
import {Button, Form} from "react-bootstrap";
import { useHistory } from 'react-router-dom';


export const CreateInquiry = (props) => {
    return (
        <Form>
            <Form.Group controlId="formBasicEmail">
                <Form.Label>Email address</Form.Label>
                <Form.Control type="email" placeholder="Enter email" />
                <Form.Text className="text-muted">
                    We'll never share your email with anyone else.
                </Form.Text>
            </Form.Group>

            <Form.Group controlId="formBasicPassword">
                <Form.Label>Password</Form.Label>
                <Form.Control type="password" placeholder="Password" />
            </Form.Group>
            <Form.Group controlId="formBasicCheckbox">
                <Form.Check type="checkbox" label="Check me out" />
            </Form.Group>
            <Button variant="primary" type="submit">
                Submit
            </Button>
        </Form>
    );
};