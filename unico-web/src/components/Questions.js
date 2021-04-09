import React from 'react';
import {Form, FormControl} from 'react-bootstrap';
import '../styles/questions.css'


export const Questions = ({question}) => {
    return (
        <div className={"p-3"}>
            <Form className={"text-justify"}>
                <Form.Label>{question.title}</Form.Label>
                {(question?.images?.length >0) &&

                    <div className={"m-3"}>
                        <img src={question?.images[0]?.url}
                             alt={question?.images[0]?.name}
                             width={150}
                        />
                    </div>
                }

                {
                    (question?.inputType?.description === "Resposta Aberta")
                    &&
                    <Form.Control as="textarea"
                                  rows={3}
                    />
                }

                {
                    (question?.inputType?.description === "resposta Fechada")
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
                    (question?.inputType?.description === "Lista de escolha unica")
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
                    (question?.inputType?.description === "Lista de escolha multipla")
                    &&
                    question?.questionOptions?.map(op=>
                        <Form.Check
                            //disabled
                            type={'checkbox'}
                            label={op.description}
                            id={op.id}
                        />)
                }







            </Form>

        </div>
    );
};