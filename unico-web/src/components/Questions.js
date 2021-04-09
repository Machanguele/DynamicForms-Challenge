import React from 'react';
import {Form, FormControl} from 'react-bootstrap';
import '../styles/questions.css'


export const Questions = ({question}) => {
    return (
        <div className={"p-3"}>
            <Form className={"text-justify"}>
                <Form.Label>{question.title}</Form.Label>
                {
                    (question?.inputType?.description === "Short answer")
                    &&
                    <FormControl type="text"/>
                }
                {
                    (question?.inputType?.description === "Paragraph")
                    &&
                    <Form.Control as="textarea"
                                  rows={3}
                    />
                }

                {
                    (question?.inputType?.description === "Unique choice radio")
                    &&
                    question?.questionOptions?.map(op=>
                        <Form.Check
                            //disabled
                            type={'radio'}
                            label={op.description}
                            id={op.id}
                        />)
                }

                {
                    (question?.inputType?.description === "Unique choice Options List")
                    &&
                    <Form.Control
                        as="select"
                        className="mr-sm-2"
                        id="inlineFormCustomSelect"
                        custom
                    >
                        {
                            question?.questionOptions?.map(op=>
                                <option value={op.id}>{op.description}</option>
                            )
                        }
                    </Form.Control>
                }

                {
                    (question?.inputType?.description === "Multiple choice")
                    &&
                    question?.questionOptions?.map(op=>
                        <Form.Check
                            //disabled
                            type={'radio'}
                            label={op.description}
                            id={op.id}
                        />)
                }

                {
                    (question?.inputType?.description === "File Upload")
                    &&
                    <Form.File
                        id="custom-file"
                        label="Upload Files"
                        custom
                    />

                }





            </Form>

        </div>
    );
};