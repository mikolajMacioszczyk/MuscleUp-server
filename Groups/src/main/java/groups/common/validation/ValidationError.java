package groups.common.validation;

import org.springframework.http.HttpStatus;

public record ValidationError(HttpStatus response, String description) {
}
