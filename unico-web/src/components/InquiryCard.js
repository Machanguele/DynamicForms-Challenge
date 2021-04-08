import {Card, Button} from "react-bootstrap";
import React from "react";

export const InquiryCard = ({description, id}) => {
    return (
            <>
                <Card style={{ width: '18rem', margin: '2%' }}>
                    <Card.Body>
                        <Card.Title>Inquiry</Card.Title>
                        <Card.Text>
                            {description}
                        </Card.Text>
                        <Button variant="primary">Go somewhere</Button>
                    </Card.Body>
                </Card></>

    );
};