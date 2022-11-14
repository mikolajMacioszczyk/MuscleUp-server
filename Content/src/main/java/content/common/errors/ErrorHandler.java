package content.common.errors;

import content.common.innerCommunicators.errors.AuthHeaderException;
import content.common.innerCommunicators.errors.InnerCommunicationException;
import content.security.UnauthorizedException;
import org.springframework.http.HttpHeaders;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.context.request.WebRequest;
import org.springframework.web.servlet.mvc.method.annotation.ResponseEntityExceptionHandler;

import static org.springframework.http.HttpStatus.INTERNAL_SERVER_ERROR;
import static org.springframework.http.HttpStatus.UNAUTHORIZED;

@ControllerAdvice
public class ErrorHandler extends ResponseEntityExceptionHandler {

    private static final String INNER_COMMUNICATION_EXCEPTION_MESSAGE = "Connection to other service failed";
    private static final String AUTH_HEADER_EXCEPTION_MESSAGE = "Header with jwtToken not found";
    private static final String UNAUTHORIZED_EXCEPTION_MESSAGE = "JWT token is incorrect";


    @ExceptionHandler(value = InnerCommunicationException.class)
    protected ResponseEntity<?> handleInnerCommunicationException(RuntimeException exception, WebRequest request) {

        return handleExceptionInternal(
                exception,
                INNER_COMMUNICATION_EXCEPTION_MESSAGE,
                new HttpHeaders(),
                INTERNAL_SERVER_ERROR,
                request
        );
    }

    @ExceptionHandler(value = AuthHeaderException.class)
    protected ResponseEntity<?> handleAuthHeaderException(RuntimeException exception, WebRequest request) {

        return handleExceptionInternal(
                exception,
                AUTH_HEADER_EXCEPTION_MESSAGE,
                new HttpHeaders(),
                UNAUTHORIZED,
                request
        );
    }

    @ExceptionHandler(value = UnauthorizedException.class)
    protected ResponseEntity<?> handleUnauthorizedException(RuntimeException exception, WebRequest request) {

        return handleExceptionInternal(
                exception,
                UNAUTHORIZED_EXCEPTION_MESSAGE,
                new HttpHeaders(),
                UNAUTHORIZED,
                request
        );
    }
}
