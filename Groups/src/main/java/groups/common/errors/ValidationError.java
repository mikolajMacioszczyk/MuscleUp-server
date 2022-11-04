package groups.common.errors;

import org.springframework.http.HttpStatus;

public record ValidationError(HttpStatus response, String description) {
}
